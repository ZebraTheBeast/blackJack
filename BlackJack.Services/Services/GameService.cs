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
using BlackJack.BusinessLogic.Mappers;

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
			_handRepository = handRepository;
			_playerRepository = playerRepository;
			_playerInGameRepository = playerInGameRepository;
			_gameRepository = gameRepository;
			_cardProvider = cardProvider;

			_logger = LogManager.GetCurrentClassLogger();

		}

		public async Task<GetGameViewModel> GetGame(long gameId)
		{
			var getGameMapper = new GetGameMapper();
			Game game = await _gameRepository.GetByIdAsync(gameId);
			var deck = await GetInGameDeck(gameId);

			var getGameViewModel = getGameMapper.GetViewModel(game, deck);

			if (getGameViewModel.Human.Points <= Constant.MinPointsValueToPlay)
			{
				await _playerRepository.RestorePlayerPoints(getGameViewModel.Human.Id);
				getGameViewModel.Human.Points = Constant.DefaultPointsValue;
			}

			return getGameViewModel;
		}


		public async Task<ResponseBetGameViewModel> PlaceBet(RequestBetGameViewModel requestBetGameViewModel)
		{	
			if (requestBetGameViewModel.BetValue <= 0)
			{
				throw new Exception(StringHelper.NoBetValue());
			}
			
			var responseBetGameViewModel = new ResponseBetGameViewModel();
			
			long humanId = await _playerInGameRepository.GetHumanIdByGameId(requestBetGameViewModel.GameId);
			Player human = await _playerRepository.GetByIdAsync(humanId);

			await _handRepository.RemoveAllCardsInHand(requestBetGameViewModel.GameId);

			if (human.Points < requestBetGameViewModel.BetValue)
			{
				throw new Exception(StringHelper.NotEnoughPoints(requestBetGameViewModel.BetValue));
			}

			int humanBetValue = await _playerInGameRepository.GetBetByPlayerId(humanId, requestBetGameViewModel.GameId);

			if (humanBetValue != 0)
			{
				throw new Exception(StringHelper.AlreadyBet());
			}

			await _playerInGameRepository.PlaceBet(human.Id, requestBetGameViewModel.BetValue, requestBetGameViewModel.GameId);
			_logger.Log(LogHelper.GetEvent(human.Id, requestBetGameViewModel.GameId, StringHelper.PlayerPlaceBet(requestBetGameViewModel.BetValue)));

			List<long> botsId = await _playerInGameRepository.GetBotsIdByGameId(requestBetGameViewModel.GameId);

			foreach (var botId in botsId)
			{
				_logger.Log(LogHelper.GetEvent(botId, requestBetGameViewModel.GameId, StringHelper.PlayerPlaceBet(Constant.BotsBetValue)));
			}

			await _playerInGameRepository.PlaceBet(botsId, requestBetGameViewModel.GameId);

			List<long> deck = await GetInGameDeck(requestBetGameViewModel.GameId);
			List<long> playersId = await _playerInGameRepository.GetAllPlayersIdByGameId(requestBetGameViewModel.GameId);

			foreach (var playerId in playersId)
			{
				deck = await GiveCardFromDeck(playerId, deck, requestBetGameViewModel.GameId);
				deck = await GiveCardFromDeck(playerId, deck, requestBetGameViewModel.GameId);
			}

			var getGameViewModel = await GetGame(requestBetGameViewModel.GameId);
			getGameViewModel.Options = StringHelper.OptionDrawCard;

			if ((getGameViewModel.Human.Hand.CardsInHandValue >= Constant.WinValue)
				|| (getGameViewModel.Dealer.Hand.CardsInHandValue >= Constant.WinValue))
			{
				StandGameViewModel standGameViewModel = await Stand(requestBetGameViewModel.GameId);
				getGameViewModel = Mapper.Map<StandGameViewModel, GetGameViewModel>(standGameViewModel);
			}

			responseBetGameViewModel = Mapper.Map<GetGameViewModel, ResponseBetGameViewModel>(getGameViewModel);
			return responseBetGameViewModel;
		}

		public async Task<DrawGameViewModel> DrawCard(long gameId)
		{
			
			long humanId = await _playerInGameRepository.GetHumanIdByGameId(gameId);
			List<long> deck = await GetInGameDeck(gameId);

			int humanBetValue = await _playerInGameRepository.GetBetByPlayerId(humanId, gameId);

			if (humanBetValue == 0)
			{
				throw new Exception(StringHelper.NoBetValue());
			}

			deck = await GiveCardFromDeck(humanId, deck, gameId);

			var getGameViewModel = await GetGame(gameId);
			getGameViewModel.Options = StringHelper.OptionDrawCard;

			if (getGameViewModel.Human.Hand.CardsInHandValue >= Constant.WinValue)
			{
				StandGameViewModel standGameViewModel = await Stand(gameId);
				getGameViewModel = Mapper.Map<StandGameViewModel, GetGameViewModel>(standGameViewModel);
			}

			var drawGameViewModel = Mapper.Map<GetGameViewModel, DrawGameViewModel>(getGameViewModel);

			return drawGameViewModel;
		}

		public async Task<StandGameViewModel> Stand(long gameId)
		{
			var botsId = new List<long>();
			var standGameViewModel = new StandGameViewModel();
			var message = string.Empty;
			var getGameViewModel = await GetGame(gameId);

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
				await UpdateScore(getGameViewModel.Bots[i], getGameViewModel.Dealer.Hand.CardsInHandValue, gameId);
				botsId.Add(getGameViewModel.Bots[i].Id);
			}

			await _playerInGameRepository.AnnulBet(botsId, gameId);

			message = await UpdateScore(getGameViewModel.Human, getGameViewModel.Dealer.Hand.CardsInHandValue, gameId);
			await _playerInGameRepository.AnnulBet(getGameViewModel.Human.Id, gameId);

			getGameViewModel = await GetGame(gameId);

			getGameViewModel.Options = StringHelper.OptionSetBet(message);

			standGameViewModel = Mapper.Map<GetGameViewModel, StandGameViewModel>(getGameViewModel);

			return standGameViewModel;
		}

		private async Task<List<long>> GetInGameDeck(long gameId)
		{
			List<long> cardsInGameId = await _handRepository.GetCardsIdByGameId(gameId);
			var deck = await _cardProvider.LoadInGameDeck(cardsInGameId);
			return deck;
		}

		private async Task<bool> BotTurn(long botId, List<long> deck, long gameId)
		{
			HandViewModelItem hand = await GetPlayerHand(botId, gameId);
			var value = hand.CardsInHandValue;

			if (value >= Constant.ValueToStopDraw)
			{
				return false;
			}

			deck = await GiveCardFromDeck(botId, deck, gameId);

			return await BotTurn(botId, deck, gameId);
		}

		private async Task<HandViewModelItem> GetPlayerHand(long playerId, long gameId)
		{
			var hand = new HandViewModelItem
			{
				CardsInHand = new List<CardViewModelItem>()
			};

			List<long> cardsIdInPlayersHand = await _handRepository.GetCardsIdByPlayerId(playerId, gameId);

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

		private async Task<List<long>> GiveCardFromDeck(long playerId, List<long> deck, long gameId)
		{
			await _handRepository.GiveCardToPlayerInGame(playerId, deck[0], gameId);
			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerDrawCard(deck[0])));
			deck.Remove(deck[0]);
			return deck;
		}

		private async Task<string> UpdateScore(PlayerViewModelItem player, int dealerCardsValue, long gameId)
		{
			_logger.Log(LogHelper.GetEvent(player.Id, gameId, StringHelper.PlayerValue(player.Hand.CardsInHandValue, dealerCardsValue)));

			if ((player.Hand.CardsInHandValue > dealerCardsValue)
			&& (player.Hand.CardsInHandValue <= Constant.WinValue))
			{
				await UpdateWonPlayersPoints(player.Id, gameId, player.BetValue);
				return StringHelper.OptionWin;
			}

			if ((player.Hand.CardsInHandValue <= Constant.WinValue)
			&& (dealerCardsValue > Constant.WinValue))
			{
				await UpdateWonPlayersPoints(player.Id, gameId, player.BetValue);
				return StringHelper.OptionWin;
			}

			if (player.Hand.CardsInHandValue > Constant.WinValue)
			{
				await UpdateLostPlayersPoints(player.Id, gameId, player.BetValue);
				return StringHelper.OptionLose;
			}

			if ((dealerCardsValue > player.Hand.CardsInHandValue)
			&& (dealerCardsValue <= Constant.WinValue))
			{
				await UpdateLostPlayersPoints(player.Id, gameId, player.BetValue);
				return StringHelper.OptionLose;
			}

			_logger.Log(LogHelper.GetEvent(player.Id, gameId, StringHelper.PlayerDraw()));
			return StringHelper.OptionDraw;
		}

		private async Task UpdateLostPlayersPoints(long playerId, long gameId, int playerBetValue)
		{
			Player player = await _playerRepository.GetByIdAsync(playerId);
			int newPointsValue = player.Points - playerBetValue;

			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerLose(playerBetValue)));

			await _playerRepository.UpdatePlayerPoints(playerId, newPointsValue);
		}

		private async Task UpdateWonPlayersPoints(long playerId, long gameId, int playerBetValue)
		{
			Player player = await _playerRepository.GetByIdAsync(playerId);
			int newPointsValue = player.Points + playerBetValue;

			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerWin(playerBetValue)));
			await _playerRepository.UpdatePlayerPoints(playerId, newPointsValue);
		}
	}
}