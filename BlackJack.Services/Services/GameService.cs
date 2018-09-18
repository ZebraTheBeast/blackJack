using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.ViewModels;
using BlackJack.Configurations;
using BlackJack.BusinessLogic.Helpers;
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
		ICardProvider _cardProvider;

		IPlayerInGameRepository _playerInGameRepository;
		IGameRepository _gameRepository;
		IHandRepository _handRepository;
		IPlayerRepository _playerRepository;

		public GameService(ICardProvider cardProvider, IHandRepository handRepository, IPlayerRepository playerRepository, IPlayerInGameRepository playerInGameRepository, IGameRepository gameRepository)
		{
			var path = string.Empty;

			_handRepository = handRepository;
			_playerRepository = playerRepository;
			_playerInGameRepository = playerInGameRepository;
			_gameRepository = gameRepository;
			_cardProvider = cardProvider;

			path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\"));
			LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(path + "BlackJack.Configuration\\Nlog.config", true);
		}

		public async Task<GetGameViewModel> GetGame(int gameId)
		{
			Logger logger = LogManager.GetCurrentClassLogger();
			try
			{
				var getGameViewModel = new GetGameViewModel();

				Game game = await _gameRepository.GetGameById(gameId);

				IEnumerable<int> cardsList = await _handRepository.GetCardIdListByGameId(game.Id);
				IEnumerable<int> bots = new List<int>();

				getGameViewModel.Human = Mapper.Map<Player, PlayerViewModel>(game.Human);
				getGameViewModel.Human.BetValue = await _playerInGameRepository.GetBetByPlayerId(game.Human.Id, game.Id);
				getGameViewModel.Human.Hand = await GetPlayerHand(game.Human.Id, game.Id);
				getGameViewModel.Dealer = Mapper.Map<Player, DealerViewModel>(await _playerRepository.GetByName(Constant.DealerName));
				getGameViewModel.Dealer.Hand = await GetPlayerHand(getGameViewModel.Dealer.Id, game.Id);
				getGameViewModel.Deck = await _cardProvider.LoadDeck(cardsList);
				getGameViewModel.Bots = new List<PlayerViewModel>();

				bots = await _playerInGameRepository.GetBotsInGame(game.Id, game.Human.Id, getGameViewModel.Dealer.Id);

				foreach (var botId in bots)
				{
					var player = await _playerRepository.GetById(botId);
					getGameViewModel.Bots.Add(Mapper.Map<Player, PlayerViewModel>(player));
				}

				foreach (var bot in getGameViewModel.Bots)
				{
					bot.Hand = await GetPlayerHand(bot.Id, game.Id);
					bot.BetValue = await _playerInGameRepository.GetBetByPlayerId(bot.Id, game.Id);
				}

				if (getGameViewModel.Human.Hand.CardList.Count() != 0)
				{
					getGameViewModel.Options = OptionHelper.OptionDrawCard();
				}

				if ((getGameViewModel.Human.Hand.CardList.Count() == 0)
					|| (getGameViewModel.Human.BetValue == 0))
				{
					getGameViewModel.Options = OptionHelper.OptionSetBet(string.Empty);
				}

				return getGameViewModel;
			}
			catch (Exception exception)
			{
				logger.Error(exception.Message);
				throw exception;
			}
		}

		public async Task<ResponseBetGameViewModel> PlaceBet(RequestBetGameViewModel requestBetGameViewModel)
		{
			Logger logger = LogManager.GetCurrentClassLogger();
			try
			{
				if (requestBetGameViewModel.BetValue <= 0)
				{
					throw new Exception(StringHelper.NoBetValue());
				}

				var responseBetGameViewModel = new ResponseBetGameViewModel();
				Game game = await _gameRepository.GetGameById(requestBetGameViewModel.GameId);
				GetGameViewModel getGameViewModel = await GetGame(game.Id);
				IEnumerable<int> playersIdList = await _playerInGameRepository.GetAll(game.Id);

				await _handRepository.RemoveAll(game.Id);

				if (getGameViewModel.Human.Points < requestBetGameViewModel.BetValue)
				{
					throw new Exception(StringHelper.NotEnoughPoints(requestBetGameViewModel.BetValue));
				}

				if (getGameViewModel.Human.BetValue != 0)
				{
					throw new Exception(StringHelper.AlreadyBet());
				}

				await _playerInGameRepository.PlaceBet(game.Human.Id, requestBetGameViewModel.BetValue, game.Id);
				logger.Log(LogHelper.GetEvent(game.Human.Id, game.Id, StringHelper.PlayerPlaceBet(requestBetGameViewModel.BetValue)));

				foreach (var bot in getGameViewModel.Bots)
				{
					await _playerInGameRepository.PlaceBet(bot.Id, Constant.BotsBetValue, game.Id);
					logger.Log(LogHelper.GetEvent(bot.Id, game.Id, StringHelper.PlayerPlaceBet(Constant.BotsBetValue)));
				}

				foreach (var playerId in playersIdList)
				{
					for (var i = 0; i < Constant.NumberStartCard; i++)
					{
						await GiveCardFromDeck(playerId, getGameViewModel.Deck[0], game.Id);
						getGameViewModel.Deck.Remove(getGameViewModel.Deck[0]);
					}
				}

				getGameViewModel = await GetGame(game.Id);
				getGameViewModel.Options = OptionHelper.OptionDrawCard();

				if ((getGameViewModel.Human.Hand.CardListValue >= Constant.WinValue)
					|| (getGameViewModel.Dealer.Hand.CardListValue >= Constant.WinValue))
				{
					StandGameViewModel standGameViewModel = await Stand(game.Id);
					getGameViewModel = Mapper.Map<StandGameViewModel, GetGameViewModel>(standGameViewModel);
				}

				responseBetGameViewModel = Mapper.Map<GetGameViewModel, ResponseBetGameViewModel>(getGameViewModel);
				return responseBetGameViewModel;
			}
			catch (Exception exception)
			{
				logger.Error(exception.Message);
				throw exception;
			}
		}

		public async Task<DrawGameViewModel> DrawCard(int gameId)
		{
			Logger logger = LogManager.GetCurrentClassLogger();
			try
			{
				var drawGameViewModel = new DrawGameViewModel();
				GetGameViewModel getGameViewModel = new GetGameViewModel();
				Game game = await _gameRepository.GetGameById(gameId);
				PlayerViewModel human = Mapper.Map<Player, PlayerViewModel>(game.Human);
				IEnumerable<int> cardsInGameList = await _handRepository.GetCardIdListByGameId(game.Id);
				List<int> deck = await _cardProvider.LoadDeck(cardsInGameList);

				human.BetValue = await _playerInGameRepository.GetBetByPlayerId(human.Id, game.Id);

				if (human.BetValue == 0)
				{
					throw new Exception(StringHelper.NoBetValue());
				}

				await GiveCardFromDeck(game.Human.Id, deck[0], game.Id);
				deck.Remove(deck[0]);

				getGameViewModel = await GetGame(game.Id);
				getGameViewModel.Options = OptionHelper.OptionDrawCard();

				if (getGameViewModel.Human.Hand.CardListValue >= Constant.WinValue)
				{
					StandGameViewModel standGameViewModel = await Stand(game.Id);
					getGameViewModel = Mapper.Map<StandGameViewModel, GetGameViewModel>(standGameViewModel);
				}

				drawGameViewModel = Mapper.Map<GetGameViewModel, DrawGameViewModel>(getGameViewModel);

				return drawGameViewModel;
			}
			catch (Exception exception)
			{
				logger.Error(exception.Message);
				throw exception;
			}
		}

		public async Task<StandGameViewModel> Stand(int gameId)
		{
			Logger logger = LogManager.GetCurrentClassLogger();
			try
			{
				var standGameViewModel = new StandGameViewModel();
				string message = string.Empty;
				Game game = await _gameRepository.GetGameById(gameId);
				GetGameViewModel getGameViewModel = await GetGame(game.Id);

				if (getGameViewModel.Human.BetValue == 0)
				{
					throw new Exception(StringHelper.NoBetValue());
				}

				if ((getGameViewModel.Dealer.Hand.CardListValue != Constant.WinValue)
					|| (getGameViewModel.Dealer.Hand.CardList.Count() != Constant.NumberCardForBlackJack))
				{
					for (var i = 0; i < getGameViewModel.Bots.Count(); i++)
					{
						await BotTurn(getGameViewModel.Bots[i].Id, getGameViewModel.Deck, game.Id);
					}
				}

				await BotTurn(getGameViewModel.Dealer.Id, getGameViewModel.Deck, game.Id);
				getGameViewModel.Dealer.Hand = await GetPlayerHand(getGameViewModel.Dealer.Id, game.Id);

				for (var i = 0; i < getGameViewModel.Bots.Count(); i++)
				{
					getGameViewModel.Bots[i].Hand = await GetPlayerHand(getGameViewModel.Bots[i].Id, game.Id);
					await UpdateScore(getGameViewModel.Bots[i].Id, getGameViewModel.Bots[i].BetValue, getGameViewModel.Bots[i].Hand.CardListValue, getGameViewModel.Dealer.Hand.CardListValue, game.Id);
					await _playerInGameRepository.AnnulBet(getGameViewModel.Bots[i].Id, game.Id);
				}

				message = await UpdateScore(getGameViewModel.Human.Id, getGameViewModel.Human.BetValue, getGameViewModel.Human.Hand.CardListValue, getGameViewModel.Dealer.Hand.CardListValue, game.Id);
				await _playerInGameRepository.AnnulBet(game.Human.Id, game.Id);

				getGameViewModel = await GetGame(game.Id);

				getGameViewModel.Options = OptionHelper.OptionSetBet(message);

				standGameViewModel = Mapper.Map<GetGameViewModel, StandGameViewModel>(getGameViewModel);

				return standGameViewModel;
			}
			catch (Exception exception)
			{
				logger.Error(exception.Message);
				throw new Exception(exception.Message);
			}
		}

		private async Task<bool> BotTurn(int botId, List<int> deck, int gameId)
		{
			try
			{
				HandViewModel hand = await GetPlayerHand(botId, gameId);
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

		private async Task<HandViewModel> GetPlayerHand(int playerId, int gameId)
		{
			var hand = new HandViewModel
			{
				CardList = new List<CardViewModel>()
			};

			IEnumerable<int> cardsIdList = await _handRepository.GetCardIdList(playerId, gameId);

			foreach (var cardId in cardsIdList)
			{
				Card card = await _cardProvider.GetCardById(cardId);
				CardViewModel cardViewModel = Mapper.Map<Card, CardViewModel>(card);
				hand.CardList.Add(cardViewModel);
			}

			foreach (var card in hand.CardList)
			{
				hand.CardListValue += card.Value;
			}

			foreach (var card in hand.CardList)
			{
				if ((card.Title.Replace(" ", string.Empty) == Constant.AceCardTitle)
					&& (hand.CardListValue > Constant.WinValue))
				{
					hand.CardListValue -= Constant.ImageCardValue;
				}
			}

			return hand;
		}

		private async Task GiveCardFromDeck(int playerId, int cardId, int gameId)
		{
			Logger logger = LogManager.GetCurrentClassLogger();
			await _handRepository.GiveCardToPlayer(playerId, cardId, gameId);
			logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerDrawCard(cardId)));
		}

		private async Task<string> UpdateScore(int playerId, int playerBetValue, int playerCardsValue, int dealerCardsValue, int gameId)
		{
			Logger logger = LogManager.GetCurrentClassLogger();
			logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerValue(playerCardsValue, dealerCardsValue)));

			if ((playerCardsValue > dealerCardsValue)
			&& (playerCardsValue <= Constant.WinValue))
			{
				await UpdateWonPlayersPoints(playerId, gameId, playerBetValue);
				return OptionHelper.OptionWin();
			}

			if ((playerCardsValue <= Constant.WinValue)
			&& (dealerCardsValue > Constant.WinValue))
			{
				await UpdateWonPlayersPoints(playerId, gameId, playerBetValue);
				return OptionHelper.OptionWin();
			}

			if (playerCardsValue > Constant.WinValue)
			{
				await UpdateLostPlayersPoints(playerId, gameId, playerBetValue);
				return OptionHelper.OptionLose();
			}

			if ((dealerCardsValue > playerCardsValue)
			&& (dealerCardsValue <= Constant.WinValue))
			{
				await UpdateLostPlayersPoints(playerId, gameId, playerBetValue);
				return OptionHelper.OptionLose();
			}

			logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerDraw()));
			return OptionHelper.OptionDraw();
		}

		private async Task UpdateLostPlayersPoints(int playerId, int gameId, int playerBetValue)
		{
			Logger logger = LogManager.GetCurrentClassLogger();
			Player player = await _playerRepository.GetById(playerId);
			int newPointsValue = player.Points - playerBetValue;

			logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerLose(playerBetValue)));

			await _playerRepository.UpdatePoints(playerId, newPointsValue);
		}

		private async Task UpdateWonPlayersPoints(int playerId, int gameId, int playerBetValue)
		{
			Logger logger = LogManager.GetCurrentClassLogger();
			Player player = await _playerRepository.GetById(playerId);
			int newPointsValue = player.Points + playerBetValue;

			logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerWin(playerBetValue)));
			await _playerRepository.UpdatePoints(playerId, newPointsValue);
		}
	}
}