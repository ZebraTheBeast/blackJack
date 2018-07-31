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
            var handRepository = new HandRepository();
            var playerRepository = new PlayerRepository();
            var playerInGameRepository = new PlayerInGameRepository();
            _deckService = new DeckService( handRepository, playerRepository, playerInGameRepository);
            _handService = new HandService(handRepository, playerInGameRepository);
            _playerService = new PlayerService(playerRepository, playerInGameRepository);
            _scoreService = new ScoreService(playerInGameRepository, playerRepository);
        }

        public async Task<GameViewModel> GetGameViewModel()
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
            cardIdList = _deckService.GetNewRefreshedDeck();
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

        private async Task MakeBet(int playerId, int betValue)
        {
            await _playerService.MakeBet(playerId, betValue);
        }

        public async Task<GameViewModel> PlaceBet(int humanId, int betValue)
        {
            var deck = new List<int>();
            IEnumerable<int> playersId = new List<int>(); ;
            var bots = new List<PlayerViewModel>();

            deck = await EndTurn();

            playersId = await _playerService.GetPlayersIdInGame();
            bots = await _playerService.GetBotsInGame();

            for (var i = 0; i < bots.Count(); i++)
            {
                await _playerService.MakeBet(bots[i].Id, Constant.BotsBetValue);
            }

            await _playerService.MakeBet(humanId, betValue);

            foreach (var playerId in playersId)
            {
                await _deckService.GiveCardFromDeck(playerId, deck[0]);
                deck.Remove(deck[0]);
                await _deckService.GiveCardFromDeck(playerId, deck[0]);
                deck.Remove(deck[0]);
            }

            var gameViewModel = await GetGameViewModel();

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
            gameViewModel.Options = OptionHelper.OptionSetBet("");

            return gameViewModel;
        }

        public async Task<GameViewModel> Draw(int humanId, List<int> deck)
        {
            await _deckService.GiveCardFromDeck(humanId, deck[0]);
            deck.Remove(deck[0]);
            var gameViewModel = await GetGameViewModel();
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
            var gameViewModel = new GameViewModel();

            await BotTurn(dealer.Id, deck);
            dealer.Hand = await _handService.GetPlayerHand(dealer.Id);
            human.Hand.CardListValue = await _handService.GetPlayerHandValue(human.Id);

            var message = await UpdateScore(human.Id, human.Hand.CardListValue, dealer.Hand.CardListValue);

            if ((dealer.Hand.CardListValue != Constant.WinValue) && dealer.Hand.CardList.Count() != Constant.NumberCardForBlackJack)
            {
                for (var i = 0; i < bots.Count(); i++)
                {
                    await BotTurn(bots[i].Id, deck);
                }
            }

            for (var i = 0; i < bots.Count(); i++)
            {
                bots[i].Hand.CardListValue = await _handService.GetPlayerHandValue(bots[i].Id);
                await UpdateScore(bots[i].Id, bots[i].Hand.CardListValue, dealer.Hand.CardListValue);
            }

            gameViewModel = await GetGameViewModel();
            gameViewModel.Options = OptionHelper.OptionSetBet(message);
            gameViewModel.Deck = deck;

            return gameViewModel;
        }
    }
}
