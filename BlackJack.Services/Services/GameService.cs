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

		public async Task<GetGameGameView> GetGame(long gameId)
		{
			var getGameMapper = new GetGameMapper();
			Game game = await _gameRepository.GetById(gameId);
            var playerIds = game.PlayersInGame.Select(playerInGame => playerInGame.Id).ToList();

            var playersInGame = await _playerInGameRepository.GetPlayersInGamePlayerIds(playerIds);

            var deck = await GetDeckInGame(gameId);

			var getGameView = getGameMapper.GetView(game, playersInGame, deck);

			if (getGameView.Human.Points <= Constant.MinPointsValueToPlay)
			{
				await _playerRepository.UpdatePlayersPoints(new List<long> { getGameView.Human.Id }, Constant.DefaultPointsValue);
				getGameView.Human.Points = Constant.DefaultPointsValue;
			}

			return getGameView;
		}


		public async Task<ResponseBetGameView> PlaceBet(RequestBetGameView requestBetGameView)
		{	
			if (requestBetGameView.BetValue <= 0)
			{
				throw new Exception(StringHelper.NoBetValue);
			}
			
			var responseBetGameView = new ResponseBetGameView();
			
			long humanId = await _playerInGameRepository.GetHumanIdByGameId(requestBetGameView.GameId);
			Player human = await _playerRepository.GetById(humanId);

			await _handRepository.RemoveAllCardsInHand(requestBetGameView.GameId);

			if (human.Points < requestBetGameView.BetValue)
			{
				throw new Exception(StringHelper.NotEnoughPoints(requestBetGameView.BetValue));
			}

			int humanBetValue = await _playerInGameRepository.GetBetByPlayerId(humanId, requestBetGameView.GameId);

			if (humanBetValue != 0)
			{
				throw new Exception(StringHelper.AlreadyBet);
			}

			await _playerInGameRepository.UpdateBet(new List<long> { human.Id }, requestBetGameView.GameId, requestBetGameView.BetValue);
			_logger.Log(LogHelper.GetEvent(human.Id, requestBetGameView.GameId, StringHelper.PlayerPlaceBet(requestBetGameView.BetValue)));

			List<long> botsId = await _playerInGameRepository.GetBotsIdByGameId(requestBetGameView.GameId);

			foreach (var botId in botsId)
			{
				_logger.Log(LogHelper.GetEvent(botId, requestBetGameView.GameId, StringHelper.PlayerPlaceBet(Constant.BotsBetValue)));
			}

			await _playerInGameRepository.UpdateBet(botsId, requestBetGameView.GameId, Constant.BotsBetValue);

			List<long> deck = await GetDeckInGame(requestBetGameView.GameId);
			List<long> playersId = await _playerInGameRepository.GetAllPlayersIdByGameId(requestBetGameView.GameId);

			foreach (var playerId in playersId)
			{
				deck = await GiveCardToPlayer(playerId, deck, requestBetGameView.GameId);
				deck = await GiveCardToPlayer(playerId, deck, requestBetGameView.GameId);
			}

			var getGameView = await GetGame(requestBetGameView.GameId);
			getGameView.Options = StringHelper.OptionDrawCard;

			if ((getGameView.Human.Hand.CardsInHandValue >= Constant.WinValue)
				|| (getGameView.Dealer.Hand.CardsInHandValue >= Constant.WinValue))
			{
				StandGameView standGameView = await Stand(requestBetGameView.GameId);
				getGameView = Mapper.Map<StandGameView, GetGameGameView>(standGameView);
			}

			responseBetGameView = Mapper.Map<GetGameGameView, ResponseBetGameView>(getGameView);
			return responseBetGameView;
		}

		public async Task<DrawGameView> DrawCard(long gameId)
		{
			
			long humanId = await _playerInGameRepository.GetHumanIdByGameId(gameId);
			List<long> deck = await GetDeckInGame(gameId);

			int humanBetValue = await _playerInGameRepository.GetBetByPlayerId(humanId, gameId);

			if (humanBetValue == 0)
			{
				throw new Exception(StringHelper.NoBetValue);
			}

			deck = await GiveCardToPlayer(humanId, deck, gameId);

			var getGameView = await GetGame(gameId);
			getGameView.Options = StringHelper.OptionDrawCard;

			if (getGameView.Human.Hand.CardsInHandValue >= Constant.WinValue)
			{
				StandGameView standGameView = await Stand(gameId);
				getGameView = Mapper.Map<StandGameView, GetGameGameView>(standGameView);
			}

			var drawGameView = Mapper.Map<GetGameGameView, DrawGameView>(getGameView);

			return drawGameView;
		}

		public async Task<StandGameView> Stand(long gameId)
		{
			var playerIds = new List<long>();
			var standGameView = new StandGameView();
			var message = string.Empty;
			var getGameView = await GetGame(gameId);
            var deck = await _cardProvider.GetCardsByIds(getGameView.Deck);

			if (getGameView.Human.BetValue == 0)
			{
				throw new Exception(StringHelper.NoBetValue);
			}

			if ((getGameView.Dealer.Hand.CardsInHandValue != Constant.WinValue)
				|| (getGameView.Dealer.Hand.CardsInHand.Count() != Constant.NumberCardForBlackJack))
			{
				for (var i = 0; i < getGameView.Bots.Count(); i++)
				{
                    var botHand = await GetPlayerHand(getGameView.Bots[i].Id, gameId);
					await BotTurn(getGameView.Bots[i].Id, deck, gameId, botHand);
				}
			}

            var dealerHand = await GetPlayerHand(getGameView.Dealer.Id, gameId);
            await BotTurn(getGameView.Dealer.Id, deck, gameId, dealerHand);

			getGameView.Dealer.Hand = await GetPlayerHand(getGameView.Dealer.Id, gameId);

			for (var i = 0; i < getGameView.Bots.Count(); i++)
			{
				getGameView.Bots[i].Hand = await GetPlayerHand(getGameView.Bots[i].Id, gameId);
				await UpdateScore(getGameView.Bots[i], getGameView.Dealer.Hand.CardsInHandValue, gameId);
				playerIds.Add(getGameView.Bots[i].Id);
			}

            playerIds.Add(getGameView.Human.Id);

			message = await UpdateScore(getGameView.Human, getGameView.Dealer.Hand.CardsInHandValue, gameId);

			await _playerInGameRepository.UpdateBet(playerIds, gameId, 0);

			getGameView = await GetGame(gameId);

			getGameView.Options = StringHelper.OptionSetBet(message);

			standGameView = Mapper.Map<GetGameGameView, StandGameView>(getGameView);

			return standGameView;
		}

		private async Task<List<long>> GetDeckInGame(long gameId)
		{
			List<long> cardsInGameId = await _handRepository.GetCardsIdByGameId(gameId);
			var deck = await _cardProvider.LoadInGameDeck(cardsInGameId);
			return deck;
		}

        private async Task<bool> BotTurn(long botId, List<Card> deck, long gameId, HandViewItem hand)
        {
            if(hand.CardsInHandValue >= Constant.ValueToStopDraw)
            {
                await GiveMultipleCards(botId, hand.CardsInHand, gameId);
                return false;
            }

            var card = Mapper.Map<Card, CardViewItem>(deck[0]);
            _logger.Log(LogHelper.GetEvent(botId, gameId, StringHelper.PlayerDrawCard(deck[0].Id)));
            deck.Remove(deck[0]);
            hand.CardsInHand.Add(card);
            hand.CardsInHandValue = CalculateCardsValue(hand.CardsInHand);

            return await BotTurn(botId, deck, gameId, hand);
        }

        private int CalculateCardsValue(List<CardViewItem> cards)
        {
            var cardsInHandValue = 0;

            foreach (var card in cards)
            {
                cardsInHandValue += card.Value;
            }

            foreach (var card in cards)
            {
                if ((card.Title.Replace(" ", string.Empty) == Constant.AceCardTitle)
                    && (cardsInHandValue > Constant.WinValue))
                {
                    cardsInHandValue -= Constant.ImageCardValue;
                }
            }
            return cardsInHandValue;
        }

        private async Task GiveMultipleCards(long playerId, List<CardViewItem> cards, long gameId)
        {
            var cardsInHandId = await _handRepository.GetCardsIdByPlayerId(playerId, gameId);
            var newCards = new List<long>();
            var hands = new List<Hand>();
            var playerInGameId = await _playerInGameRepository.GetIdByPlayerIdAndGameId(playerId, gameId);

            foreach(var card in cards)
            {
                if (!cardsInHandId.Contains(card.Id))
                {
                    newCards.Add(card.Id);
                }
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

		private async Task<HandViewItem> GetPlayerHand(long playerId, long gameId)
		{
			var hand = new HandViewItem
			{
				CardsInHand = new List<CardViewItem>()
			};

			List<long> cardsIdInPlayersHand = await _handRepository.GetCardsIdByPlayerId(playerId, gameId);

			var cards = await _cardProvider.GetCardsByIds(cardsIdInPlayersHand);
			hand.CardsInHand = Mapper.Map<List<Card>, List<CardViewItem>>(cards);

            hand.CardsInHandValue = CalculateCardsValue(hand.CardsInHand);

			return hand;
		}

		private async Task<List<long>> GiveCardToPlayer(long playerId, List<long> deck, long gameId)
		{
			await _handRepository.GiveCardToPlayerInGame(playerId, deck[0], gameId);
			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerDrawCard(deck[0])));
			deck.Remove(deck[0]);
			return deck;
		}

		private async Task<string> UpdateScore(PlayerViewItem player, int dealerCardsValue, long gameId)
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

			_logger.Log(LogHelper.GetEvent(player.Id, gameId, StringHelper.PlayerDraw));
			return StringHelper.OptionDraw;
		}

		private async Task UpdateLostPlayersPoints(long playerId, long gameId, int playerBetValue)
		{
			Player player = await _playerRepository.GetById(playerId);
			int newPointsValue = player.Points - playerBetValue;

			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerLose(playerBetValue)));

			await _playerRepository.UpdatePlayersPoints(new List<long> { playerId }, newPointsValue);
		}

		private async Task UpdateWonPlayersPoints(long playerId, long gameId, int playerBetValue)
		{
			Player player = await _playerRepository.GetById(playerId);
			int newPointsValue = player.Points + playerBetValue;

			_logger.Log(LogHelper.GetEvent(playerId, gameId, StringHelper.PlayerWin(playerBetValue)));

			await _playerRepository.UpdatePlayersPoints(new List<long> { playerId }, newPointsValue);
		}
	}
}