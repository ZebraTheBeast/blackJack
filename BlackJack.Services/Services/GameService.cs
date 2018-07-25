using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.ViewModel;
using BlackJack.DAL.Repository;
using BlackJack.Configuration.Constant;

namespace BlackJack.BLL.Services
{
    public class GameService : IGameService
    {
        IDeckService _deckService;
        IHandService _handService;
        IPlayerService _playerService;
        IScoreService _scoreService;

        public GameService()
        {
            var deckRepository = new DeckRepository();
            var cardRepository = new CardRepository();
            var handRepository = new HandRepository();
            var playerRepository = new PlayerRepository();
            var playerInGameRepository = new PlayerInGameRepository();

            _deckService = new DeckService(deckRepository, cardRepository, handRepository);
            _handService = new HandService(handRepository, cardRepository, playerInGameRepository);
            _playerService = new PlayerService(playerRepository, playerInGameRepository);
            _scoreService = new ScoreService(playerInGameRepository, playerRepository);
        }

        public GameViewModel GetGameViewModel()
        {
            var gameViewModel = new GameViewModel();
            gameViewModel.Bots = _playerService.GetBotsInGame();

            for(var i = 0; i < gameViewModel.Bots.Count; i++)
            {
                gameViewModel.Bots[i].Hand = _handService.GetPlayerHand(gameViewModel.Bots[i].Id);
            }

            gameViewModel.Human = _playerService.GetHumanInGame();
            gameViewModel.Human.Hand = _handService.GetPlayerHand(gameViewModel.Human.Id);

            gameViewModel.Dealer = _playerService.GetDealer();
            gameViewModel.Dealer.Hand = _handService.GetPlayerHand(gameViewModel.Dealer.Id);

            gameViewModel.Deck = _deckService.GetDeck();

            return gameViewModel;
        }

        public void StartGame(string humanName)
        {
            EndTurn();
            _playerService.RemoveAllPlayers();
            _playerService.SetPlayerToGame(humanName);
        }

        public void Dealing()
        {
            var playersId = _playerService.GetPlayersIdInGame();
            var bots = _playerService.GetBotsInGame();

            for(var i = 0; i < bots.Count(); i ++)
            {
                _playerService.MakeBet(bots[i].Id, Constant.BotsBetValue);
            }

            foreach(var playerId in playersId)
            {
                _deckService.GiveCardFromDeck(playerId);
                _deckService.GiveCardFromDeck(playerId);
            }
        }

        public void HumanDrawCard(int humanId)
        {
            _deckService.GiveCardFromDeck(humanId);
        }

        public string UpdateScore(int playerId, int playerCardsValue, int dealerCardsValue)
        {
            var message = _scoreService.UpdateScore(playerId, playerCardsValue, dealerCardsValue);
            return message;
        }

        public void EndTurn()
        {
            _handService.RemoveAllCardsInHand();
            _deckService.RefreshAndShuffleDeck();
        }

        public bool BotTurn(int botId)
        {
            var value = _handService.GetPlayerHandValue(botId);
            if (value >= Constant.ValueToStopDraw)
            {
                return false;
            }
            _deckService.GiveCardFromDeck(botId);
            return BotTurn(botId);
        }

        public bool MakeBet(int playerId, int betValue)
        {
            var response = _playerService.MakeBet(playerId, betValue);
            return response;
        }
    }
}
