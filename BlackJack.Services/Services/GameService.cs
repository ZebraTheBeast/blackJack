using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.ViewModels;
using BlackJack.Configurations;
using BlackJack.BusinessLogic.Helpers;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.DataAccess.Interfaces;
using NLog;
using AutoMapper;
using BlackJack.Entities;

namespace BlackJack.BusinessLogic.Services
{
	public class GameService : IGameService
	{
		private ICardProvider _cardProvider;

		private IPlayerInGameRepository _playerInGameRepository;
		private IGameRepository _gameRepository;
		private IHandRepository _handRepository;
		private IPlayerRepository _playerRepository;

		private Logger _logger;

		public GameService(ICardProvider cardProvider, IHandRepository handRepository, IPlayerRepository playerRepository, IPlayerInGameRepository playerInGameRepository, IGameRepository gameRepository)
		{
			var path = string.Empty;

			_handRepository = handRepository;
			_playerRepository = playerRepository;
			_playerInGameRepository = playerInGameRepository;
			_gameRepository = gameRepository;
			_cardProvider = cardProvider;

			_logger = LogManager.GetCurrentClassLogger();
		}

		public async Task<GetGameViewModel> GetGame(int gameId)
		{
			var getGameViewModel = new GetGameViewModel();
			var humanCards = new List<Card>();
			Game game = await _gameRepository.GetGameById(gameId);
			var human = game.PlayersInGame.Where(player => player.IsHuman == true).FirstOrDefault();
			List<int> cardsInGameId = await _handRepository.GetCardsIdByGameId(game.Id);
			var botsId = new List<int>();

			getGameViewModel.Human = Mapper.Map<Player, PlayerViewModelItem>(human.Player);
			getGameViewModel.Human.BetValue = human.BetValue;

			if (getGameViewModel.Human.Points <= Constant.MinPointsValueToPlay)
			{
				await _playerRepository.RestorePlayerPoints(getGameViewModel.Human.Id);
				getGameViewModel.Human.Points = Constant.DefaultPointsValue;
			}

			getGameViewModel.Human.Hand = await GetPlayerHand(getGameViewModel.Human.Id, gameId);

			getGameViewModel.Dealer = Mapper.Map<Player, DealerViewModelItem>(await _playerRepository.GetPlayerByName(Constant.DealerName));
			getGameViewModel.Dealer.Hand = await GetPlayerHand(getGameViewModel.Dealer.Id, game.Id);
			getGameViewModel.Deck = await _cardProvider.LoadDeck(cardsInGameId);
			getGameViewModel.Bots = new List<PlayerViewModelItem>();

			botsId = await _playerInGameRepository.GetBotsIdByGameId(game.Id, getGameViewModel.Dealer.Id);
			var bots = await _playerRepository.GetPlayersByIds(botsId);
			var botsInGame = await _playerInGameRepository.GetPlayersInGame(botsId, game.Id);

			getGameViewModel.Bots.AddRange(Mapper.Map<List<Player>, List<PlayerViewModelItem>>(bots));

			foreach (var bot in getGameViewModel.Bots)
			{
				bot.Hand = await GetPlayerHand(bot.Id, game.Id);
			}

			foreach (var bot in botsInGame)
			{
				getGameViewModel.Bots.Where(item => item.Id == bot.PlayerId).FirstOrDefault().BetValue = bot.BetValue;
			}

			if (getGameViewModel.Human.Hand.CardsInHand.Count() != 0)
			{
				getGameViewModel.Options = OptionHelper.OptionDrawCard;
			}

			if ((getGameViewModel.Human.Hand.CardsInHand.Count() == 0)
				|| (getGameViewModel.Human.BetValue == 0))
			{
				getGameViewModel.Options = OptionHelper.OptionSetBet(string.Empty);
			}

			return getGameViewModel;
		}


		public async Task<ResponseBetGameViewModel> PlaceBet(RequestBetGameViewModel requestBetGameViewModel)
		{
			if (requestBetGameViewModel.BetValue <= 0)
			{
				throw new Exception(StringHelper.NoBetValue());
			}
			var botsId = new List<int>();
			var responseBetGameViewModel = new ResponseBetGameViewModel();
			var getGameViewModel = new GetGameViewModel();

			List<int> cardsList = await _handRepository.GetCardsIdByGameId(requestBetGameViewModel.GameId);
			List<int> deck = await _cardProvider.LoadDeck(cardsList);
			int humanId = await _playerInGameRepository.GetHumanIdByGameId(requestBetGameViewModel.GameId);
			Player human = await _playerRepository.GetPlayerById(humanId);
			var dealer = await _playerRepository.GetPlayerByName(Constant.DealerName);
			int humanBetValue = await _playerInGameRepository.GetBetByPlayerId(humanId, requestBetGameViewModel.GameId);
			IEnumerable<int> playersId = await _playerInGameRepository.GetAllPlayersIdByGameId(requestBetGameViewModel.GameId);

			await _handRepository.RemoveAllCardsInHand(requestBetGameViewModel.GameId);

			if (human.Points < requestBetGameViewModel.BetValue)
			{
				throw new Exception(StringHelper.NotEnoughPoints(requestBetGameViewModel.BetValue));
			}

			if (humanBetValue != 0)
			{
				throw new Exception(StringHelper.AlreadyBet());
			}

			await _playerInGameRepository.PlaceBet(human.Id, requestBetGameViewModel.BetValue, requestBetGameViewModel.GameId);
			_logger.Log(LogHelper.GetEvent(human.Id, requestBetGameViewModel.GameId, StringHelper.PlayerPlaceBet(requestBetGameViewModel.BetValue)));

			botsId = await _playerInGameRepository.GetBotsIdByGameId(requestBetGameViewModel.GameId, dealer.Id);

			foreach (var botId in botsId)
			{
				_logger.Log(LogHelper.GetEvent(botId, requestBetGameViewModel.GameId, StringHelper.PlayerPlaceBet(Constant.BotsBetValue)));
			}

			await _playerInGameRepository.PlaceBet(botsId, requestBetGameViewModel.GameId);

			foreach (var playerId in playersId)
			{
				for (var i = 0; i < Constant.NumberStartCard; i++)
				{
					await GiveCardFromDeck(playerId, deck[0], requestBetGameViewModel.GameId);
					deck.Remove(deck[0]);
				}
			}

			getGameViewModel = await GetGame(requestBetGameViewModel.GameId);
			getGameViewModel.Options = OptionHelper.OptionDrawCard;

			if ((getGameViewModel.Human.Hand.CardsInHandValue >= Constant.WinValue)
				|| (getGameViewModel.Dealer.Hand.CardsInHandValue >= Constant.WinValue))
			{
				StandGameViewModel standGameViewModel = await Stand(requestBetGameViewModel.GameId);
				getGameViewModel = Mapper.Map<StandGameViewModel, GetGameViewModel>(standGameViewModel);
			}

			responseBetGameViewModel = Mapper.Map<GetGameViewModel, ResponseBetGameViewModel>(getGameViewModel);
			return responseBetGameViewModel;
		}

		public async Task<DrawGameViewModel> DrawCard(int gameId)
		{
			var drawGameViewModel = new DrawGameViewModel();
			var getGameViewModel = new GetGameViewModel();
			int humanId = await _playerInGameRepository.GetHumanIdByGameId(gameId);
			List<int> cardsInGameId = await _handRepository.GetCardsIdByGameId(gameId);
			List<int> deck = await _cardProvider.LoadDeck(cardsInGameId);

			int humanBetValue = await _playerInGameRepository.GetBetByPlayerId(humanId, gameId);

			if (humanBetValue == 0)
			{
				throw new Exception(StringHelper.NoBetValue());
			}

			await GiveCardFromDeck(humanId, deck[0], gameId);
			deck.Remove(deck[0]);

			getGameViewModel = await GetGame(gameId);
			getGameViewModel.Options = OptionHelper.OptionDrawCard;

			if (getGameViewModel.Human.Hand.CardsInHandValue >= Constant.WinValue)
			{
				StandGameViewModel standGameViewModel = await Stand(gameId);
				getGameViewModel = Mapper.Map<StandGameViewModel, GetGameViewModel>(standGameViewModel);
			}

			drawGameViewModel = Mapper.Map<GetGameViewModel, DrawGameViewModel>(getGameViewModel);

			return drawGameViewModel;
		}

		public async Task<StandGameViewModel> Stand(int gameId)
		{
			var botsId = new List<int>();
			var standGameViewModel = new StandGameViewModel();
			string message = string.Empty;
			GetGameViewModel getGameViewModel = await GetGame(gameId);

			if (getGameViewModel.Human.BetValue == 0)
			{
				throw new Exception(StringHelper.NoBetValue());
			}

			if ((getGameViewModel.Dealer.Hand.CardsInHandValue != Constant.WinValue)
				|| (getGameViewModel.Dealer.Hand.CardsInHand.Count() != Constant.NumberCardForBlackJack))
			{
				for (var i = 0; i < getGameViewModel.Bots.Count(); i++)
				{
					await BotTurn(getGameViewModel.Bots[i].Id, getGameViewModel.Deck, gameId);
				}
			}

			await BotTurn(getGameViewModel.Dealer.Id, getGameViewModel.Deck, gameId);
			getGameViewModel.Dealer.Hand = await GetPlayerHand(getGameViewModel.Dealer.Id, gameId);

			for (var i = 0; i < getGameViewModel.Bots.Count(); i++)
			{
				getGameViewModel.Bots[i].Hand = await GetPlayerHand(getGameViewModel.Bots[i].Id, gameId);
				await UpdateScore(getGameViewModel.Bots[i].Id, getGameViewModel.Bots[i].BetValue, getGameViewModel.Bots[i].Hand.CardsInHandValue, getGameViewModel.Dealer.Hand.CardsInHandValue, gameId);
				botsId.Add(getGameViewModel.Bots[i].Id);
			}

			await _playerInGameRepository.AnnulBet(botsId, gameId);

			message = await UpdateScore(getGameViewModel.Human.Id, getGameViewModel.Human.BetValue, getGameViewModel.Human.Hand.CardsInHandValue, getGameViewModel.Dealer.Hand.CardsInHandValue, gameId);
			await _playerInGameRepository.AnnulBet(getGameViewModel.Human.Id, gameId);

			getGameViewModel = await GetGame(gameId);

			getGameViewModel.Options = OptionHelper.OptionSetBet(message);

			standGameViewModel = Mapper.Map<GetGameViewModel, StandGameViewModel>(getGameViewModel);

			return standGameViewModel;
		}

		private async Task<bool> BotTurn(int botId, List<int> deck, int gameId)
		{
			HandViewModelItem hand = await GetPlayerHand(botId, gameId);
			var value = hand.CardsInHandValue;

			if (value >= Constant.ValueToStopDraw)
			{
				return false;
			}

			await GiveCardFromDeck(botId, deck[0], gameId);
			deck.Remove(deck[0]);

			return await BotTurn(botId, deck, gameId);
		}

		private async Task<HandViewModelItem> GetPlayerHand(int playerId, int gameId)
		{
			var hand = new HandViewModelItem
			{
				CardsInHand = new List<CardViewModelItem>()
			};

			List<int> cardsIdInPlayersHand = await _handRepository.GetCardsIdByPlayerId(playerId, gameId);

			var cards = await _cardProvider.GetCardsByIds(cardsIdInPlayersHand);
			hand.CardsInHand = Mapper.Map<List<Card>, List<CardViewModelItem>>(cards);

			foreach (var card in hand.CardsInHand)
			{
				hand.CardsInHandValue += card.Value;
			}

			foreach (var card in hand.CardsInHand)
			{
				if ((card.Title.Replace(" ", string.Empty) == Constant.AceCardTitle)
					&& (hand.CardsInHandValue > Constant.WinValue))
				{
					hand.CardsInHandValue -= Constant.ImageCardValue;
				}
			}

			return hand;
		}

		private async Task GiveCardFromDeck(int playerId, int cardId, int gameId)
		{
			await _handRepository.GiveCardToPlayerInGame(playerId, cardId, gameId);
			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerDrawCard(cardId)));
		}

		private async Task<string> UpdateScore(int playerId, int playerBetValue, int playerCardsValue, int dealerCardsValue, int gameId)
		{
			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerValue(playerCardsValue, dealerCardsValue)));

			if ((playerCardsValue > dealerCardsValue)
			&& (playerCardsValue <= Constant.WinValue))
			{
				await UpdateWonPlayersPoints(playerId, gameId, playerBetValue);
				return OptionHelper.OptionWin;
			}

			if ((playerCardsValue <= Constant.WinValue)
			&& (dealerCardsValue > Constant.WinValue))
			{
				await UpdateWonPlayersPoints(playerId, gameId, playerBetValue);
				return OptionHelper.OptionWin;
			}

			if (playerCardsValue > Constant.WinValue)
			{
				await UpdateLostPlayersPoints(playerId, gameId, playerBetValue);
				return OptionHelper.OptionLose;
			}

			if ((dealerCardsValue > playerCardsValue)
			&& (dealerCardsValue <= Constant.WinValue))
			{
				await UpdateLostPlayersPoints(playerId, gameId, playerBetValue);
				return OptionHelper.OptionLose;
			}

			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerDraw()));
			return OptionHelper.OptionDraw;
		}

		private async Task UpdateLostPlayersPoints(int playerId, int gameId, int playerBetValue)
		{
			Player player = await _playerRepository.GetPlayerById(playerId);
			int newPointsValue = player.Points - playerBetValue;

			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerLose(playerBetValue)));

			await _playerRepository.UpdatePlayerPoints(playerId, newPointsValue);
		}

		private async Task UpdateWonPlayersPoints(int playerId, int gameId, int playerBetValue)
		{
			Player player = await _playerRepository.GetPlayerById(playerId);
			int newPointsValue = player.Points + playerBetValue;

			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerWin(playerBetValue)));
			await _playerRepository.UpdatePlayerPoints(playerId, newPointsValue);
		}
	}
}