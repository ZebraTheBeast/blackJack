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
                gameViewModel.Human = await _playerService.GetHumanInGame();
                gameViewModel.Dealer = await _playerService.GetDealer();
                gameViewModel.Deck = await _deckService.LoadDeck();

                if (gameViewModel.Human.Hand.CardList.Count() != 0)
                {
                    gameViewModel.Options = OptionHelper.OptionDrawCard();
                }

                if ((gameViewModel.Human.Hand.CardList.Count() == 0)
                    || (gameViewModel.Human.Hand.BetValue == 0))
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

        private async Task PlaceBet(int playerId, int betValue)
        {
            try
            {
                await _playerService.PlaceBet(playerId, betValue);
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
                var gameViewModel = new GameViewModel();
                var deck = new List<int>();
                var bots = await _playerService.GetBotsInGame();
                var human = await _playerService.GetHumanInGame();
                var playersIdList = await _playerService.GetPlayersIdInGame();

                if (human.Hand.BetValue != 0)
                {
                    throw new Exception(StringHelper.AlreadyBet());
                }

                await _handService.RemoveAllCardsInHand();
                deck = _deckService.GetNewRefreshedDeck();

                await _playerService.PlaceBet(human.Id, betValue);

                for (var i = 0; i < bots.Count(); i++)
                {
                    await _playerService.PlaceBet(bots[i].Id, Constant.BotsBetValue);
                }

                foreach (var playerId in playersIdList)
                {
                    await _deckService.GiveCardFromDeck(playerId, deck[0]);
                    deck.Remove(deck[0]);
                    await _deckService.GiveCardFromDeck(playerId, deck[0]);
                    deck.Remove(deck[0]);
                }

                gameViewModel = await GetGameViewModel();
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

                if (human.Hand.BetValue == 0)
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

                dealer.Hand.CardListValue = await _handService.GetPlayerHandValue(dealer.Id);

                if (human.Hand.BetValue == 0)
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