using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.ViewModels;
using BlackJack.Configurations;
using BlackJack.BusinessLogic.Helper;
using BlackJack.BusinessLogic.Interfaces;
using System.IO;

namespace BlackJack.BusinessLogic.Services
{
	public class GameService : IGameService
	{
		IDeckProvider _deckProvider;
		IHandProvider _handProvider;
		IPlayerProvider _playerProvider;
		IScoreProvider _scoreProvider;

		public GameService(IDeckProvider deckProvider, IHandProvider handProvider, IPlayerProvider playerProvider, IScoreProvider scoreProvider)
		{
			_deckProvider = deckProvider;
			_handProvider = handProvider;
			_playerProvider = playerProvider;
			_scoreProvider = scoreProvider;

			var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\"));
			NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(path + "BlackJack.Configuration\\Nlog.config", true);
		}

		public async Task<GameViewModel> GetGameViewModel(int gameId)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			try
			{
				var gameViewModel = new GameViewModel
				{
					Bots = await _playerProvider.GetBotsInGame(gameId),
					Human = await _playerProvider.GetHumanInGame(gameId),
					Dealer = await _playerProvider.GetDealer(gameId),
					Deck = await _deckProvider.LoadDeck(gameId)
				};

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
				logger.Error(exception.Message);
				throw exception;
			}
		}

		private async Task<bool> BotTurn(int botId, List<int> deck, int gameId)
		{
			try
			{
				var value = await _handProvider.GetPlayerHandValue(botId, gameId);

				if (value >= Constant.ValueToStopDraw)
				{
					return false;
				}

				await _deckProvider.GiveCardFromDeck(botId, deck[0], gameId);
				deck.Remove(deck[0]);

				return await BotTurn(botId, deck, gameId);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		public async Task<GameViewModel> PlaceBet(int betValue, int humanId)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			try
			{
				var gameViewModel = new GameViewModel();
				var deck = new List<int>();
				var bots = await _playerProvider.GetBotsInGame(humanId);
				var human = await _playerProvider.GetHumanInGame(humanId);
				var playersIdList = await _playerProvider.GetPlayersIdInGame(humanId);

				if (human.Hand.BetValue != 0)
				{
					throw new Exception(StringHelper.AlreadyBet());
				}

				await _handProvider.RemoveAllCardsInHand(humanId);
				deck = _deckProvider.GetNewRefreshedDeck();

				await _playerProvider.PlaceBet(human.Id, betValue, humanId);


				for (var i = 0; i < bots.Count(); i++)
				{
					await _playerProvider.PlaceBet(bots[i].Id, Constant.BotsBetValue, humanId);
				}

				foreach (var playerId in playersIdList)
				{
					await _deckProvider.GiveCardFromDeck(playerId, deck[0], humanId);
					deck.Remove(deck[0]);
					await _deckProvider.GiveCardFromDeck(playerId, deck[0], humanId);
					deck.Remove(deck[0]);
				}

				gameViewModel = await GetGameViewModel(humanId);
				gameViewModel.Options = OptionHelper.OptionDrawCard();

				if ((gameViewModel.Human.Hand.CardListValue >= Constant.WinValue)
					|| (gameViewModel.Dealer.Hand.CardListValue >= Constant.WinValue))
				{
					gameViewModel = await Stand(humanId);
				}

				return gameViewModel;
			}
			catch (Exception exception)
			{
				logger.Error(exception.Message);
				throw exception;
			}
		}

		public async Task<GameViewModel> Draw(int gameId)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			try
			{
				var human = await _playerProvider.GetHumanInGame(gameId);
				var deck = await _deckProvider.LoadDeck(gameId);

				if (human.Hand.BetValue == 0)
				{
					throw new Exception(StringHelper.NoBetValue());
				}

				await _deckProvider.GiveCardFromDeck(human.Id, deck[0], gameId);
				deck.Remove(deck[0]);

				var gameViewModel = await GetGameViewModel(gameId);
				gameViewModel.Options = OptionHelper.OptionDrawCard();

				if (gameViewModel.Human.Hand.CardListValue >= Constant.WinValue)
				{
					gameViewModel = await Stand(gameId);
				}

				return gameViewModel;
			}
			catch (Exception exception)
			{
				logger.Error(exception.Message);
				throw exception;
			}
		}

		public async Task<GameViewModel> Stand(int gameId)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			try
			{
				var gameViewModel = new GameViewModel();
				var human = await _playerProvider.GetHumanInGame(gameId);
				var bots = await _playerProvider.GetBotsInGame(gameId);
				var dealer = await _playerProvider.GetDealer(gameId);
				var deck = await _deckProvider.LoadDeck(gameId);

				if (human.Hand.BetValue == 0)
				{
					throw new Exception(StringHelper.NoBetValue());
				}

				if ((dealer.Hand.CardListValue != Constant.WinValue)
					|| (dealer.Hand.CardList.Count() != Constant.NumberCardForBlackJack))
				{
					for (var i = 0; i < bots.Count(); i++)
					{
						await BotTurn(bots[i].Id, deck, gameId);
					}
				}

				await BotTurn(dealer.Id, deck, gameId);
				dealer.Hand = await _handProvider.GetPlayerHand(dealer.Id, gameId);

				for (var i = 0; i < bots.Count(); i++)
				{
					bots[i].Hand.CardListValue = await _handProvider.GetPlayerHandValue(bots[i].Id, gameId);
					await _scoreProvider.UpdateScore(bots[i].Id, bots[i].Hand.CardListValue, dealer.Hand.CardListValue, gameId);
				}

				var message = await _scoreProvider.UpdateScore(human.Id, human.Hand.CardListValue, dealer.Hand.CardListValue, gameId);

				gameViewModel = await GetGameViewModel(gameId);
				gameViewModel.Options = OptionHelper.OptionSetBet(message);

				return gameViewModel;
			}
			catch (Exception exception)
			{
				logger.Error(exception.Message);
				throw new Exception(exception.Message);
			}
		}
	}
}