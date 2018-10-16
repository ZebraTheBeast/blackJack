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
			Game game = await _gameRepository.GetById(gameId);
			var deck = await GetInGameDeck(gameId);

			var getGameViewModel = getGameMapper.GetViewModel(game, deck);

			if (getGameViewModel.Human.Points <= Constant.MinPointsValueToPlay)
			{
				await _playerRepository.UpdatePlayerPoints(getGameViewModel.Human.Id, Constant.DefaultPointsValue);
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
			Player human = await _playerRepository.GetById(humanId);

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

			await _playerInGameRepository.UpdateBet(human.Id, requestBetGameViewModel.GameId, requestBetGameViewModel.BetValue);
			_logger.Log(LogHelper.GetEvent(human.Id, requestBetGameViewModel.GameId, StringHelper.PlayerPlaceBet(requestBetGameViewModel.BetValue)));

			List<long> botsId = await _playerInGameRepository.GetBotsIdByGameId(requestBetGameViewModel.GameId);

			foreach (var botId in botsId)
			{
				_logger.Log(LogHelper.GetEvent(botId, requestBetGameViewModel.GameId, StringHelper.PlayerPlaceBet(Constant.BotsBetValue)));
			}

			await _playerInGameRepository.UpdateBet(botsId, requestBetGameViewModel.GameId, Constant.BotsBetValue);

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
            var deck = await _cardProvider.GetCardsByIds(getGameViewModel.Deck);

			if (getGameViewModel.Human.BetValue == 0)
			{
				throw new Exception(StringHelper.NoBetValue());
			}

			if ((getGameViewModel.Dealer.Hand.CardsInHandValue != Constant.WinValue)
				|| (getGameViewModel.Dealer.Hand.CardsInHand.Count() != Constant.NumberCardForBlackJack))
			{
				for (var i = 0; i < getGameViewModel.Bots.Count(); i++)
				{
                    var botHand = await GetPlayerHand(getGameViewModel.Bots[i].Id, gameId);
					await BotTurn(getGameViewModel.Bots[i].Id, deck, gameId, botHand);
				}
			}

            var dealerHand = await GetPlayerHand(getGameViewModel.Dealer.Id, gameId);
            await BotTurn(getGameViewModel.Dealer.Id, deck, gameId, dealerHand);

			getGameViewModel.Dealer.Hand = await GetPlayerHand(getGameViewModel.Dealer.Id, gameId);

			for (var i = 0; i < getGameViewModel.Bots.Count(); i++)
			{
				getGameViewModel.Bots[i].Hand = await GetPlayerHand(getGameViewModel.Bots[i].Id, gameId);
				await UpdateScore(getGameViewModel.Bots[i], getGameViewModel.Dealer.Hand.CardsInHandValue, gameId);
				botsId.Add(getGameViewModel.Bots[i].Id);
			}

			await _playerInGameRepository.UpdateBet(botsId, gameId, 0);

			message = await UpdateScore(getGameViewModel.Human, getGameViewModel.Dealer.Hand.CardsInHandValue, gameId);
			await _playerInGameRepository.UpdateBet(getGameViewModel.Human.Id, gameId, 0);

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

        private async Task<bool> BotTurn(long botId, List<Card> deck, long gameId, HandViewModelItem hand)
        {
            if(hand.CardsInHandValue >= Constant.ValueToStopDraw)
            {
                await GiveCardFromDeck(botId, hand, gameId);
                return false;
            }
            var card = Mapper.Map<Card, CardViewModelItem>(deck[0]);
            _logger.Log(LogHelper.GetEvent(botId, gameId, StringHelper.PlayerDrawCard(deck[0].Id)));
            deck.Remove(deck[0]);
            hand.CardsInHand.Add(card);
            hand = CalculateCards(hand);
            return await BotTurn(botId, deck, gameId, hand);
        }

        private HandViewModelItem CalculateCards(HandViewModelItem hand)
        {
            hand.CardsInHandValue = 0;

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

        private async Task GiveCardFromDeck(long playerId, HandViewModelItem handViewModelItem, long gameId)
        {
            var cardsId = await _handRepository.GetCardsIdByPlayerId(playerId, gameId);
            var newCards = new List<long>();
            var hands = new List<Hand>();
            var playerInGameId = await _playerInGameRepository.GetIdByPlayerIdAndGameId(playerId, gameId);

            foreach(var card in handViewModelItem.CardsInHand)
            {
                newCards.Add(card.Id);
            }
            
            foreach(var cardId in cardsId)
            {
                newCards.Remove(cardId);
            }

            foreach(var newCard in newCards)
            {
                var hand = new Hand();
                hand.CardId = newCard;
                hand.PlayerInGameId = playerInGameId;

                hands.Add(hand);
            }
            await _handRepository.Add(hands);

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

            hand = CalculateCards(hand);

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
			Player player = await _playerRepository.GetById(playerId);
			int newPointsValue = player.Points - playerBetValue;

			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerLose(playerBetValue)));

			await _playerRepository.UpdatePlayerPoints(playerId, newPointsValue);
		}

		private async Task UpdateWonPlayersPoints(long playerId, long gameId, int playerBetValue)
		{
			Player player = await _playerRepository.GetById(playerId);
			int newPointsValue = player.Points + playerBetValue;

			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerWin(playerBetValue)));
			await _playerRepository.UpdatePlayerPoints(playerId, newPointsValue);
		}
	}
}