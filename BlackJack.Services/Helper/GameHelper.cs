using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.ViewModel;
using BlackJack.DAL.Repository;
using BlackJack.Configuration.Constant;
using BlackJack.BLL.Helper;

namespace BlackJack.BLL.Services
{
    public class GameHelper : IGameHelper
    {
        DeckService _deckService;
        HandService _handService;
        PlayerService _playerService;
        ScoreService _scoreService;

        public GameHelper()
        {
            var cardRepository = new CardRepository();
            var handRepository = new HandRepository();
            var playerRepository = new PlayerRepository();
            var playerInGameRepository = new PlayerInGameRepository();

            _deckService = new DeckService(cardRepository, handRepository);
            _handService = new HandService(handRepository, cardRepository, playerInGameRepository);
            _playerService = new PlayerService(playerRepository, playerInGameRepository);
            _scoreService = new ScoreService(playerInGameRepository, playerRepository);
        }

        private GameViewModel GetGameViewModel()
        {
            var gameViewModel = new GameViewModel();
            gameViewModel.Bots = _playerService.GetBotsInGame();

            for (var i = 0; i < gameViewModel.Bots.Count; i++)
            {
                gameViewModel.Bots[i].Hand = _handService.GetPlayerHand(gameViewModel.Bots[i].Id);
            }

            gameViewModel.Human = _playerService.GetHumanInGame();
            gameViewModel.Human.Hand = _handService.GetPlayerHand(gameViewModel.Human.Id);

            gameViewModel.Dealer = _playerService.GetDealer();
            gameViewModel.Dealer.Hand = _handService.GetPlayerHand(gameViewModel.Dealer.Id);

            return gameViewModel;
        }


        private string UpdateScore(int playerId, int playerCardsValue, int dealerCardsValue)
        {
            var message = _scoreService.UpdateScore(playerId, playerCardsValue, dealerCardsValue);
            return message;
        }

        private List<int> EndTurn()
        {
            var cardIdList = new List<int>();
            _handService.RemoveAllCardsInHand();
            cardIdList = _deckService.RefreshAndShuffleDeck();
            return cardIdList;
        }

        private bool BotTurn(int botId, List<int> deck)
        {
            var value = _handService.GetPlayerHandValue(botId);
            if (value >= Constant.ValueToStopDraw)
            {
                return false;
            }
            _deckService.GiveCardFromDeck(botId, deck[0]);
            deck.Remove(deck[0]);
            return BotTurn(botId, deck);
        }

        private bool MakeBet(int playerId, int betValue)
        {
            var response = _playerService.MakeBet(playerId, betValue);
            return response;
        }

        public GameViewModel PlaceBet(int humanId, int pointsValue)
        {
            var deck = new List<int>();
            var playersId = new List<int>(); ;
            var bots = new List<PlayerViewModel>();
            var response = false;

            deck =  EndTurn();
            response = MakeBet(humanId, pointsValue);

            if (!response)
            {
                var errorGameViewModel = GetGameViewModel();
                errorGameViewModel.Options = OptionHelper.OptionErrorBet();
                return errorGameViewModel;
            }

            playersId = _playerService.GetPlayersIdInGame();
            bots = _playerService.GetBotsInGame();

            for (var i = 0; i < bots.Count(); i++)
            {
                _playerService.MakeBet(bots[i].Id, Constant.BotsBetValue);
            }

            foreach (var playerId in playersId)
            {
                _deckService.GiveCardFromDeck(playerId, deck[0]);
                deck.Remove(deck[0]);
                _deckService.GiveCardFromDeck(playerId, deck[0]);
                deck.Remove(deck[0]);
            }

            var gameViewModel = GetGameViewModel();

            gameViewModel.ButtonPushed = 1;
            gameViewModel.Options = OptionHelper.OptionDrawCard();
            gameViewModel.Deck = deck;

            if ((gameViewModel.Human.Hand.CardListValue >= Constant.WinValue) || (gameViewModel.Dealer.Hand.CardListValue >= Constant.WinValue))
            {
                gameViewModel = BotTurn(deck);
            }

            return gameViewModel;
        }

        public GameViewModel StartGame(string playerName)
        {
            var deck = new List<int>();
            deck = EndTurn();

            _playerService.SetPlayerToGame(playerName);

            var gameViewModel = new GameViewModel();


            gameViewModel = GetGameViewModel();
            gameViewModel.Deck = deck;
            gameViewModel.ButtonPushed = 0;
            gameViewModel.Options = OptionHelper.OptionSetBet("");

            return gameViewModel;
        }

        public GameViewModel Draw(int humanId, List<int> deck)
        {
            _deckService.GiveCardFromDeck(humanId, deck[0]);
            deck.Remove(deck[0]);
            var gameViewModel = GetGameViewModel();
            gameViewModel.ButtonPushed = 1;
            gameViewModel.Options = OptionHelper.OptionDrawCard();
            gameViewModel.Deck = deck;

            if (gameViewModel.Human.Hand.CardListValue >= Constant.WinValue)
            {
                gameViewModel = BotTurn(deck);
            }

            return gameViewModel;
        }

        public GameViewModel BotTurn(List<int> deck)
        {
            var bots = _playerService.GetBotsInGame();
            var dealer = _playerService.GetDealer();
            var human = _playerService.GetHumanInGame();

            BotTurn(dealer.Id, deck);
            dealer.Hand.CardListValue = _handService.GetPlayerHandValue(dealer.Id);
            human.Hand.CardListValue = _handService.GetPlayerHandValue(human.Id);

            for (var i = 0; i < bots.Count(); i++)
            {
                bots[i].Hand.CardListValue = _handService.GetPlayerHandValue(bots[i].Id);
                BotTurn(bots[i].Id, deck);
                UpdateScore(bots[i].Id, bots[i].Hand.CardListValue, dealer.Hand.CardListValue);
            }

            var message = UpdateScore(human.Id, human.Hand.CardListValue, dealer.Hand.CardListValue);

            var gameViewModel = GetGameViewModel();
            gameViewModel.Options = message;
            gameViewModel.Options = OptionHelper.OptionSetBet(message);
            gameViewModel.ButtonPushed = 0;
            gameViewModel.Deck = deck;

            return gameViewModel;
        }
    }
}
