using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.DAL.Repository;
using BlackJack.Configuration.Constant;
using BlackJack.BLL.Services;
using BlackJack.BLL.Helper;
using BlackJack.BLL.Interface;

namespace BlackJack.BLL.Providers
{
    public class GameProvider
    {
        IDeckService _deckService;
        IHandService _handService;
        IPlayerService _playerService;
        IScoreService _scoreService;


        public GameProvider()
        {
            var cardRepository = new CardRepository();
            var handRepository = new HandRepository();
            var playerRepository = new PlayerRepository();
            var playerInGameRepository = new PlayerInGameRepository();
            _deckService = new DeckService(cardRepository, handRepository, playerRepository);
            _handService = new HandService(handRepository, cardRepository, playerInGameRepository);
            _playerService = new PlayerService(playerRepository, playerInGameRepository);
            _scoreService = new ScoreService(playerInGameRepository, playerRepository);
        }

        private async Task<GameViewModel> GetGameViewModel()
        {
            var gameViewModel = new GameViewModel();
            gameViewModel.Bots = await _playerService.GetBotsInGame();

            for (var i = 0; i < gameViewModel.Bots.Count; i++)
            {
                gameViewModel.Bots[i].Hand = await _handService.GetPlayerHand(gameViewModel.Bots[i].Id);
            }

            gameViewModel.Human = await _playerService.GetHumanInGame();
            gameViewModel.Human.Hand = await _handService.GetPlayerHand(gameViewModel.Human.Id);

            gameViewModel.Dealer = await _playerService.GetDealer();
            gameViewModel.Dealer.Hand = await _handService.GetPlayerHand(gameViewModel.Dealer.Id);

            return gameViewModel;
        }


        private async Task<string> UpdateScore(int playerId, int playerCardsValue, int dealerCardsValue)
        {
            var message = await _scoreService.UpdateScore(playerId, playerCardsValue, dealerCardsValue);
            return message;
        }

        private async Task<List<int>> EndTurn()
        {
            var cardIdList = new List<int>();
            await _handService.RemoveAllCardsInHand();
            cardIdList = _deckService.RefreshAndShuffleDeck();
            return cardIdList;
        }

        private async Task<bool> BotTurn(int botId, List<int> deck)
        {
            var value = await _handService.GetPlayerHandValue(botId);
            if (value >= Constant.ValueToStopDraw)
            {
                return false;
            }
            await _deckService.GiveCardFromDeck(botId, deck[0]);
            deck.Remove(deck[0]);
            return await BotTurn(botId, deck);
        }

        private async Task<bool> MakeBet(int playerId, int betValue)
        {
            var response = await _playerService.MakeBet(playerId, betValue);
            return response;
        }

        public async Task<GameViewModel> PlaceBet(int humanId, int pointsValue)
        {
            var deck = new List<int>();
            IEnumerable<int> playersId = new List<int>(); ;
            var bots = new List<PlayerViewModel>();
            var response = false;

            deck = await EndTurn();
            response = await MakeBet(humanId, pointsValue);

            if (!response)
            {
                var errorGameViewModel = await GetGameViewModel();
                errorGameViewModel.Options = OptionHelper.OptionErrorBet();
                return errorGameViewModel;
            }

            playersId = await _playerService.GetPlayersIdInGame();
            bots = await _playerService.GetBotsInGame();

            for (var i = 0; i < bots.Count(); i++)
            {
                await _playerService.MakeBet(bots[i].Id, Constant.BotsBetValue);
            }

            foreach (var playerId in playersId)
            {
                await _deckService.GiveCardFromDeck(playerId, deck[0]);
                deck.Remove(deck[0]);
                await _deckService.GiveCardFromDeck(playerId, deck[0]);
                deck.Remove(deck[0]);
            }

            var gameViewModel = await GetGameViewModel();

            gameViewModel.ButtonPushed = 1;
            gameViewModel.Options = OptionHelper.OptionDrawCard();
            gameViewModel.Deck = deck;

            if ((gameViewModel.Human.Hand.CardListValue >= Constant.WinValue) || (gameViewModel.Dealer.Hand.CardListValue >= Constant.WinValue))
            {
                gameViewModel = await BotTurn(deck);
            }

            return gameViewModel;
        }

        public async Task<GameViewModel> StartGame(string playerName)
        {
            var deck = new List<int>();

            deck = await EndTurn();
            await _playerService.SetPlayerToGame(playerName);

            var gameViewModel = new GameViewModel();


            gameViewModel = await GetGameViewModel();
            gameViewModel.Deck = deck;
            gameViewModel.ButtonPushed = 0;
            gameViewModel.Options = OptionHelper.OptionSetBet("");

            return gameViewModel;
        }

        public async Task<GameViewModel> Draw(int humanId, List<int> deck)
        {
            await _deckService.GiveCardFromDeck(humanId, deck[0]);
            deck.Remove(deck[0]);
            var gameViewModel = await GetGameViewModel();
            gameViewModel.ButtonPushed = 1;
            gameViewModel.Options = OptionHelper.OptionDrawCard();
            gameViewModel.Deck = deck;

            if (gameViewModel.Human.Hand.CardListValue >= Constant.WinValue)
            {
                gameViewModel = await BotTurn(deck);
            }

            return gameViewModel;
        }

        public async Task<GameViewModel> BotTurn(List<int> deck)
        {
            var bots = await _playerService.GetBotsInGame();
            var dealer = await _playerService.GetDealer();
            var human = await _playerService.GetHumanInGame();

            await BotTurn(dealer.Id, deck);
            dealer.Hand.CardListValue = await _handService.GetPlayerHandValue(dealer.Id);
            human.Hand.CardListValue = await _handService.GetPlayerHandValue(human.Id);

            for (var i = 0; i < bots.Count(); i++)
            {
                await BotTurn(bots[i].Id, deck);
                bots[i].Hand.CardListValue = await _handService.GetPlayerHandValue(bots[i].Id);
                await UpdateScore(bots[i].Id, bots[i].Hand.CardListValue, dealer.Hand.CardListValue);
            }

            var message = await UpdateScore(human.Id, human.Hand.CardListValue, dealer.Hand.CardListValue);

            var gameViewModel = await GetGameViewModel();
            gameViewModel.Options = message;
            gameViewModel.Options = OptionHelper.OptionSetBet(message);
            gameViewModel.ButtonPushed = 0;
            gameViewModel.Deck = deck;

            return gameViewModel;
        }
    }
}
