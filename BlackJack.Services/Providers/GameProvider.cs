using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Configuration.Constant;
using BlackJack.BLL.Services;
using BlackJack.BLL.Helper;
using BlackJack.BLL.Interface;
using System.IO;

namespace BlackJack.BLL.Providers
{
    public class GameProvider : IGameProvider
    {
        IDeckService _deckService;
        IHandService _handService;
        IPlayerService _playerService;
        IScoreService _scoreService;

        public GameProvider(IDeckService deckService, IHandService handService, IPlayerService playerService, IScoreService scoreService)
        {
            _deckService = deckService;
            _handService = handService;
            _playerService = playerService;
            _scoreService = scoreService;


            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\"));
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(path + "BlackJack.Configuration\\Nlog.config", true);
        }

        public async Task<GameViewModel> GetGameViewModel()
        {
            try
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

                gameViewModel.Deck = await _deckService.LoadDeck();

                if (gameViewModel.Human.Hand.CardList.Count() != 0)
                {
                    gameViewModel.Options = OptionHelper.OptionDrawCard();
                }

                if ((gameViewModel.Human.Hand.CardList.Count() == 0)
                    || (gameViewModel.Human.Hand.Points == 0))
                {
                    gameViewModel.Options = OptionHelper.OptionSetBet("");
                }

                return gameViewModel;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private async Task<string> UpdateScore(int playerId, int playerCardsValue, int dealerCardsValue)
        {
            try
            {
                var message = await _scoreService.UpdateScore(playerId, playerCardsValue, dealerCardsValue);
                return message;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private async Task<bool> BotTurn(int botId, List<int> deck)
        {
            try
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
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private async Task MakeBet(int playerId, int betValue)
        {
            try
            {
                await _playerService.MakeBet(playerId, betValue);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<GameViewModel> PlaceBet(int betValue)
        {
            try
            {
                var deck = new List<int>();
                IEnumerable<int> playersId = new List<int>(); ;
                var bots = await _playerService.GetBotsInGame();

                var human = await _playerService.GetHumanInGame();
                human.Hand = await _handService.GetPlayerHand(human.Id);

                if(human.Hand.Points != 0)
                {
                    throw new Exception(StringHelper.AlreadyBet());
                }

                await _handService.RemoveAllCardsInHand();
                deck = _deckService.GetNewRefreshedDeck();

                playersId = await _playerService.GetPlayersIdInGame();
                
                await _playerService.MakeBet(human.Id, betValue);

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

                gameViewModel.Options = OptionHelper.OptionDrawCard();
                gameViewModel.Deck = deck;

                if ((gameViewModel.Human.Hand.CardListValue >= Constant.WinValue) 
                    || (gameViewModel.Dealer.Hand.CardListValue >= Constant.WinValue))
                {
                    gameViewModel = await Stand(deck);
                }

                return gameViewModel;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task StartGame(string playerName)
        {
            try
            {
                await _handService.RemoveAllCardsInHand();
                await _playerService.SetPlayerToGame(playerName);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<GameViewModel> Draw(List<int> deck)
        {
            try
            {
                var human = await _playerService.GetHumanInGame();
                human.Hand = await _handService.GetPlayerHand(human.Id);

                if (human.Hand.Points == 0)
                {
                    throw new Exception(StringHelper.NoBetValue());
                }

                await _deckService.GiveCardFromDeck(human.Id, deck[0]);
                deck.Remove(deck[0]);

                var gameViewModel = await GetGameViewModel();
                gameViewModel.Options = OptionHelper.OptionDrawCard();
                gameViewModel.Deck = deck;

                if (gameViewModel.Human.Hand.CardListValue >= Constant.WinValue)
                {
                    gameViewModel = await Stand(deck);
                }

                return gameViewModel;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<GameViewModel> Stand(List<int> deck)
        {
            try
            {
                var gameViewModel = new GameViewModel();
                var bots = await _playerService.GetBotsInGame();
                var dealer = await _playerService.GetDealer();
                var human = await _playerService.GetHumanInGame();
                
                await BotTurn(dealer.Id, deck);

                dealer.Hand = await _handService.GetPlayerHand(dealer.Id);
                human.Hand = await _handService.GetPlayerHand(human.Id);

                if(human.Hand.Points == 0)
                {
                    throw new Exception(StringHelper.NoBetValue());
                }

                var message = await UpdateScore(human.Id, human.Hand.CardListValue, dealer.Hand.CardListValue);

                if ((dealer.Hand.CardListValue != Constant.WinValue) 
                    && (dealer.Hand.CardList.Count() != Constant.NumberCardForBlackJack))
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
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}