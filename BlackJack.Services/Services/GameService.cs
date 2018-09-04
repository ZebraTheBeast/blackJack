using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.ViewModels;
using BlackJack.Configurations;
using BlackJack.BusinessLogic.Helper;
using BlackJack.BusinessLogic.Interfaces;
using System.IO;
using BlackJack.DataAccess.Interfaces;
using NLog;
using AutoMapper;
using BlackJack.Entities;

namespace BlackJack.BusinessLogic.Services
{
	public class GameService : IGameService
	{
		IPlayerProvider _playerProvider;

		IPlayerInGameRepository _playerInGameRepository;
		IGameRepository _gameRepository;
		IHandRepository _handRepository;

		public GameService(IHandRepository handRepository, IPlayerProvider playerProvider, IPlayerInGameRepository playerInGameRepository, IGameRepository gameRepository)
		{
			_handRepository = handRepository;
			_playerProvider = playerProvider;
			_playerInGameRepository = playerInGameRepository;
			_gameRepository = gameRepository;


			var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\"));
			NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(path + "BlackJack.Configuration\\Nlog.config", true);
		}

		public async Task<GameViewModel> GetGameViewModel(int humanId)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			try
			{
				var gameViewModel = new GameViewModel();

				var gameId = await _gameRepository.GetGameIdByHumanId(humanId);

				var cardsList = await _handRepository.GetCardIdListByGameId(gameId);

				gameViewModel.Human = Mapper.Map<Player, PlayerViewModel>(await _playerProvider.GetPlayerInfo(humanId));
				gameViewModel.Human.BetValue = await _playerInGameRepository.GetBetByPlayerId(gameViewModel.Human.Id, gameId);
				gameViewModel.Human.Hand = await GetPlayerHand(gameViewModel.Human.Id, gameId);
				gameViewModel.Dealer = Mapper.Map<Player, DealerViewModel>(await _playerProvider.GetDealer(gameId));
				gameViewModel.Dealer.Hand = await GetPlayerHand(gameViewModel.Dealer.Id, gameId);
				gameViewModel.Deck = CardHelper.LoadDeck(cardsList);

				var bots = await _playerInGameRepository.GetBotsInGame(gameId, gameViewModel.Human.Id, gameViewModel.Dealer.Id);
				gameViewModel.Bots = Mapper.Map<List<Player>, List<PlayerViewModel>>(await _playerProvider.GetBotsInfo(bots));

				foreach (var bot in gameViewModel.Bots)
				{
					bot.Hand = await GetPlayerHand(bot.Id, gameId);
					bot.BetValue = await _playerInGameRepository.GetBetByPlayerId(bot.Id, gameId);
				}

				if (gameViewModel.Human.Hand.CardList.Count() != 0)
				{
					gameViewModel.Options = OptionHelper.OptionDrawCard();
				}

				if ((gameViewModel.Human.Hand.CardList.Count() == 0)
					|| (gameViewModel.Human.BetValue == 0))
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
				var value = await GetPlayerHandValue(botId, gameId);

				if (value >= Constant.ValueToStopDraw)
				{
					return false;
				}

				await GiveCardFromDeck(botId, deck[0], gameId);
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
				var gameId = await _gameRepository.GetGameIdByHumanId(humanId);

				if (betValue <= 0)
				{
					throw new Exception(StringHelper.NoBetValue());
				}

				await _handRepository.RemoveAll(gameId);
				var gameViewModel = await GetGameViewModel(humanId);
				var playersIdList = await _playerInGameRepository.GetAll(gameId);

				if (gameViewModel.Human.Points < betValue)
				{
					throw new Exception(StringHelper.NotEnoughPoints(betValue));
				}

				if (gameViewModel.Human.BetValue != 0)
				{
					throw new Exception(StringHelper.AlreadyBet());
				}

				await _playerInGameRepository.PlaceBet(gameViewModel.Human.Id, betValue, gameId);
				logger.Info(StringHelper.PlayerPlaceBet(gameViewModel.Human.Id, betValue, gameId));

				foreach (var bot in gameViewModel.Bots)
				{
					await _playerInGameRepository.PlaceBet(bot.Id, Constant.BotsBetValue, gameId);
					logger.Info(StringHelper.PlayerPlaceBet(bot.Id, Constant.BotsBetValue, gameId));
				}

				foreach (var playerId in playersIdList)
				{
					for (var i = 0; i < Constant.NumberStartCard; i++)
					{
						await GiveCardFromDeck(playerId, gameViewModel.Deck[0], gameId);
						gameViewModel.Deck.Remove(gameViewModel.Deck[0]);
					}
				}

				gameViewModel = await GetGameViewModel(humanId);
				gameViewModel.Options = OptionHelper.OptionDrawCard();

				if ((gameViewModel.Human.Hand.CardListValue >= Constant.WinValue)
					|| (gameViewModel.Dealer.Hand.CardListValue >= Constant.WinValue))
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

		public async Task<GameViewModel> Draw(int humanId)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			try
			{
				var gameId = await _gameRepository.GetGameIdByHumanId(humanId);
				var human = Mapper.Map<Player, PlayerViewModel>(await _playerProvider.GetPlayerInfo(humanId));
				human.BetValue = await _playerInGameRepository.GetBetByPlayerId(human.Id, gameId);
				var cardsList = await _handRepository.GetCardIdListByGameId(gameId);
				var deck = CardHelper.LoadDeck(cardsList);

				if (human.BetValue == 0)
				{
					throw new Exception(StringHelper.NoBetValue());
				}

				await GiveCardFromDeck(human.Id, deck[0], gameId);
				deck.Remove(deck[0]);

				var gameViewModel = await GetGameViewModel(humanId);
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

		public async Task<GameViewModel> Stand(int humanId)
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
			try
			{
				var gameId = await _gameRepository.GetGameIdByHumanId(humanId);
				var gameViewModel = await GetGameViewModel(humanId);

				if (gameViewModel.Human.BetValue == 0)
				{
					throw new Exception(StringHelper.NoBetValue());
				}

				if ((gameViewModel.Dealer.Hand.CardListValue != Constant.WinValue)
					|| (gameViewModel.Dealer.Hand.CardList.Count() != Constant.NumberCardForBlackJack))
				{
					for (var i = 0; i < gameViewModel.Bots.Count(); i++)
					{
						await BotTurn(gameViewModel.Bots[i].Id, gameViewModel.Deck, gameId);
					}
				}

				await BotTurn(gameViewModel.Dealer.Id, gameViewModel.Deck, gameId);
				gameViewModel.Dealer.Hand = await GetPlayerHand(gameViewModel.Dealer.Id, gameId);

				for (var i = 0; i < gameViewModel.Bots.Count(); i++)
				{
					gameViewModel.Bots[i].Hand.CardListValue = await GetPlayerHandValue(gameViewModel.Bots[i].Id, gameId);
					await _playerProvider.UpdateScore(gameViewModel.Bots[i].Id, gameViewModel.Bots[i].BetValue, gameViewModel.Bots[i].Hand.CardListValue, gameViewModel.Dealer.Hand.CardListValue, gameId);
					await _playerInGameRepository.AnnulBet(gameViewModel.Bots[i].Id, gameId);
				}

				var message = await _playerProvider.UpdateScore(gameViewModel.Human.Id, gameViewModel.Human.BetValue, gameViewModel.Human.Hand.CardListValue, gameViewModel.Dealer.Hand.CardListValue, gameId);
				await _playerInGameRepository.AnnulBet(gameViewModel.Human.Id, gameId);

				gameViewModel = await GetGameViewModel(humanId);
				gameViewModel.Options = OptionHelper.OptionSetBet(message);

				return gameViewModel;
			}
			catch (Exception exception)
			{
				logger.Error(exception.Message);
				throw new Exception(exception.Message);
			}
		}

		private async Task<HandViewModel> GetPlayerHand(int playerId, int gameId)
		{
			var deck = CardHelper.GetFullDeck();

			var hand = new HandViewModel
			{
				CardList = new List<CardViewModel>()
			};

			var cardsIdList = await _handRepository.GetCardIdList(playerId, gameId);

			foreach (var cardId in cardsIdList)
			{
				var card = CardHelper.GetCardById(cardId, deck);
				hand.CardList.Add(card);
			}

			hand.CardListValue = CardHelper.CountCardsValue(hand.CardList);

			return hand;
		}

		private async Task<int> GetPlayerHandValue(int playerId, int gameId)
		{
			var deck = CardHelper.GetFullDeck();
			var cards = new List<CardViewModel>();
			var playerCardsIdList = await _handRepository.GetCardIdList(playerId, gameId);

			foreach (var cardId in playerCardsIdList)
			{
				var card = CardHelper.GetCardById(cardId, deck);
				cards.Add(card);
			}

			var handValue = CardHelper.CountCardsValue(cards);

			return handValue;

		}

		private async Task GiveCardFromDeck(int playerId, int cardId, int gameId)
		{
			var logger = LogManager.GetCurrentClassLogger();
			await _handRepository.GiveCardToPlayer(playerId, cardId, gameId);
			logger.Info(StringHelper.PlayerDrawCard(playerId, cardId, gameId));
		}
	}
}