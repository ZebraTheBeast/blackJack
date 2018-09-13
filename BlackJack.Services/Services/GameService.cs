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
using BlackJack.BusinessLogic.Helpers;

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

				var game = await _gameRepository.GetGameByHumanId(humanId);

				var cardsList = await _handRepository.GetCardIdListByGameId(game.Id);

				gameViewModel.Human = Mapper.Map<Player, PlayerViewModel>(game.Human);
				gameViewModel.Human.BetValue = await _playerInGameRepository.GetBetByPlayerId(game.Human.Id, game.Id);
				gameViewModel.Human.Hand = await GetPlayerHand(game.Human.Id, game.Id);
				gameViewModel.Dealer = Mapper.Map<Player, DealerViewModel>(await _playerProvider.GetDealer(game.Id));
				gameViewModel.Dealer.Hand = await GetPlayerHand(gameViewModel.Dealer.Id, game.Id);
				gameViewModel.Deck = CardHelper.LoadDeck(cardsList);

				var bots = await _playerInGameRepository.GetBotsInGame(game.Id, game.Human.Id, gameViewModel.Dealer.Id);
				gameViewModel.Bots = Mapper.Map<List<Player>, List<PlayerViewModel>>(await _playerProvider.GetBotsInfo(bots));

				foreach (var bot in gameViewModel.Bots)
				{
					bot.Hand = await GetPlayerHand(bot.Id, game.Id);
					bot.BetValue = await _playerInGameRepository.GetBetByPlayerId(bot.Id, game.Id);
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
				var hand = await GetPlayerHand(botId, gameId);
				var value = hand.CardListValue;

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
				var game = await _gameRepository.GetGameByHumanId(humanId);

				if (betValue <= 0)
				{
					throw new Exception(StringHelper.NoBetValue());
				}

				await _handRepository.RemoveAll(game.Id);
				var gameViewModel = await GetGameViewModel(game.Human.Id);
				var playersIdList = await _playerInGameRepository.GetAll(game.Id);

				if (gameViewModel.Human.Points < betValue)
				{
					throw new Exception(StringHelper.NotEnoughPoints(betValue));
				}

				if (gameViewModel.Human.BetValue != 0)
				{
					throw new Exception(StringHelper.AlreadyBet());
				}

				await _playerInGameRepository.PlaceBet(game.Human.Id, betValue, game.Id);
				logger.Log(LogHelper.GetEvent(game.Human.Id, game.Id, StringHelper.PlayerPlaceBet(betValue)));

				foreach (var bot in gameViewModel.Bots)
				{
					await _playerInGameRepository.PlaceBet(bot.Id, Constant.BotsBetValue, game.Id);
					logger.Log(LogHelper.GetEvent(bot.Id, game.Id,StringHelper.PlayerPlaceBet(Constant.BotsBetValue)));
				}

				foreach (var playerId in playersIdList)
				{
					for (var i = 0; i < Constant.NumberStartCard; i++)
					{
						await GiveCardFromDeck(playerId, gameViewModel.Deck[0], game.Id);
						gameViewModel.Deck.Remove(gameViewModel.Deck[0]);
					}
				}

				gameViewModel = await GetGameViewModel(game.Human.Id);
				gameViewModel.Options = OptionHelper.OptionDrawCard();

				if ((gameViewModel.Human.Hand.CardListValue >= Constant.WinValue)
					|| (gameViewModel.Dealer.Hand.CardListValue >= Constant.WinValue))
				{
					gameViewModel = await Stand(game.Human.Id);
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
				var game = await _gameRepository.GetGameByHumanId(humanId);
				var human = Mapper.Map<Player, PlayerViewModel>(game.Human);
				human.BetValue = await _playerInGameRepository.GetBetByPlayerId(human.Id, game.Id);
				var cardsInGameList = await _handRepository.GetCardIdListByGameId(game.Id);
				var deck = CardHelper.LoadDeck(cardsInGameList);

				if (human.BetValue == 0)
				{
					throw new Exception(StringHelper.NoBetValue());
				}

				await GiveCardFromDeck(game.Human.Id, deck[0], game.Id);
				deck.Remove(deck[0]);

				var gameViewModel = await GetGameViewModel(game.Human.Id);
				gameViewModel.Options = OptionHelper.OptionDrawCard();

				if (gameViewModel.Human.Hand.CardListValue >= Constant.WinValue)
				{
					gameViewModel = await Stand(game.Human.Id);
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
				var game = await _gameRepository.GetGameByHumanId(humanId);
				var gameViewModel = await GetGameViewModel(game.Human.Id);

				if (gameViewModel.Human.BetValue == 0)
				{
					throw new Exception(StringHelper.NoBetValue());
				}

				if ((gameViewModel.Dealer.Hand.CardListValue != Constant.WinValue)
					|| (gameViewModel.Dealer.Hand.CardList.Count() != Constant.NumberCardForBlackJack))
				{
					for (var i = 0; i < gameViewModel.Bots.Count(); i++)
					{
						await BotTurn(gameViewModel.Bots[i].Id, gameViewModel.Deck, game.Id);
					}
				}

				await BotTurn(gameViewModel.Dealer.Id, gameViewModel.Deck, game.Id);
				gameViewModel.Dealer.Hand = await GetPlayerHand(gameViewModel.Dealer.Id, game.Id);

				for (var i = 0; i < gameViewModel.Bots.Count(); i++)
				{
					gameViewModel.Bots[i].Hand = await GetPlayerHand(gameViewModel.Bots[i].Id, game.Id);
					await _playerProvider.UpdateScore(gameViewModel.Bots[i].Id, gameViewModel.Bots[i].BetValue, gameViewModel.Bots[i].Hand.CardListValue, gameViewModel.Dealer.Hand.CardListValue, game.Id);
					await _playerInGameRepository.AnnulBet(gameViewModel.Bots[i].Id, game.Id);
				}

				var message = await _playerProvider.UpdateScore(gameViewModel.Human.Id, gameViewModel.Human.BetValue, gameViewModel.Human.Hand.CardListValue, gameViewModel.Dealer.Hand.CardListValue, game.Id);
				await _playerInGameRepository.AnnulBet(game.Human.Id, game.Id);

				gameViewModel = await GetGameViewModel(game.Human.Id);

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

		private async Task GiveCardFromDeck(int playerId, int cardId, int gameId)
		{
			var logger = LogManager.GetCurrentClassLogger();
			await _handRepository.GiveCardToPlayer(playerId, cardId, gameId);
			logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerDrawCard(cardId)));
		}
	}
}