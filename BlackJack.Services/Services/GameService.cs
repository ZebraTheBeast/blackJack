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
using BlackJack.Entities.Enums;

namespace BlackJack.BusinessLogic.Services
{
	public class GameService : IGameService
	{
        private ICardRepository _cardRepository;
		private IPlayerInGameRepository _playerInGameRepository;
		private IGameRepository _gameRepository;
		private ICardInHandRepository _cardInHandRepository;
		private IPlayerRepository _playerRepository;
		private Logger _logger;

		public GameService(ICardRepository cardRepository, ICardInHandRepository cardInHandRepository, IPlayerRepository playerRepository, IPlayerInGameRepository playerInGameRepository, IGameRepository gameRepository)
		{
			_cardInHandRepository = cardInHandRepository;
			_playerRepository = playerRepository;
			_playerInGameRepository = playerInGameRepository;
			_gameRepository = gameRepository;
            _cardRepository = cardRepository;
			_logger = LogManager.GetCurrentClassLogger();
		}

        public async Task<ResponseStartGameGameView> StartGame(string playerName, int botsAmount)
        {
            Player human = await _playerRepository.GetPlayerByName(playerName);
            var playersInGame = new List<PlayerInGame>();

            if (human == null)
            {
                var player = new Player { Name = playerName, Type = PlayerType.Human, Points = Constant.DefaultPointsValue };
                await _playerRepository.Add(player);
                human = await _playerRepository.GetPlayerByName(playerName);
            }

            List<Player> bots = await _playerRepository.GetBots(botsAmount);
            Player dealer = await _playerRepository.GetDealer();
            bots.Add(dealer);
            long oldGameId = await _gameRepository.GetGameIdByHumanId(human.Id);
            var playersIdWithoutPoints = new List<long>();

            foreach (var bot in bots)
            {
                if (bot.Points < Constant.MinPointsValueToPlay)
                {
                    playersIdWithoutPoints.Add(bot.Id);
                    bot.Points = Constant.DefaultPointsValue;
                }
            }

            if (playersIdWithoutPoints.Count != 0)
            {
                await _playerRepository.UpdatePlayersPoints(playersIdWithoutPoints, Constant.DefaultPointsValue);
            }

            await RestoreCardsInDb();

            if (oldGameId != 0)
            {
                await _cardInHandRepository.RemoveAllCardsByGameId(oldGameId);
                await _playerInGameRepository.RemoveAllPlayersFromGame(oldGameId);
                await _gameRepository.DeleteGameById(oldGameId);
            }

            long gameId = await _gameRepository.Add(new Game());

            foreach (var bot in bots)
            {
                playersInGame.Add(new PlayerInGame() { PlayerId = bot.Id, GameId = gameId });
                _logger.Log(LogHelper.GetEvent(bot.Id, gameId, UserMessages.BotJoinGame));
            }

            playersInGame.Add(new PlayerInGame() { PlayerId = human.Id, GameId = gameId });
            await _playerInGameRepository.Add(playersInGame);
            _logger.Log(LogHelper.GetEvent(human.Id, gameId, UserMessages.HumanJoinGame));

            ResponseStartGameGameView responseStartGameGameView = new ResponseStartGameGameView();
            var getGameGameView = await GetGame(gameId);
            responseStartGameGameView = Mapper.Map<GetGameGameView, ResponseStartGameGameView>(getGameGameView);
            responseStartGameGameView.GameId = gameId;

            return responseStartGameGameView;
        }

        public async Task<LoadGameGameView> LoadGame(string playerName)
        {
            Player player = await _playerRepository.GetPlayerByName(playerName);

            if (player == null)
            {
                throw new Exception(UserMessages.NoLastGame);
            }

            var gameId = await _gameRepository.GetGameIdByHumanId(player.Id);
            await RestoreCardsInDb();

            if (gameId == 0)
            {
                throw new Exception(UserMessages.NoLastGame);
            }

            _logger.Log(LogHelper.GetEvent(player.Id, gameId, UserMessages.PlayerContinueGame));

            LoadGameGameView loadGameGameView = new LoadGameGameView();
            var getGameGameView = await GetGame(gameId);
            loadGameGameView = Mapper.Map<GetGameGameView, LoadGameGameView>(getGameGameView);
            loadGameGameView.GameId = gameId;
            return loadGameGameView;
        }

		public async Task<ResponseBetGameView> PlaceBet(RequestBetGameView requestBetGameView)
		{	
			if (requestBetGameView.BetValue <= 0)
			{
				throw new Exception(UserMessages.NoBetValue);   
			}
			
			var responseBetGameView = new ResponseBetGameView();
			long humanId = await _playerInGameRepository.GetHumanIdByGameId(requestBetGameView.GameId);
			Player human = await _playerRepository.GetById(humanId);
			await _cardInHandRepository.RemoveAllCardsByGameId(requestBetGameView.GameId);

			if (human.Points < requestBetGameView.BetValue)
			{
				throw new Exception(UserMessages.NotEnoughPoints(requestBetGameView.BetValue));
			}

			int humanBetValue = await _playerInGameRepository.GetBetByPlayerId(humanId, requestBetGameView.GameId);

			if (humanBetValue != 0)
			{
				throw new Exception(UserMessages.AlreadyBet);
			}

			await _playerInGameRepository.UpdateBet(new List<long> { human.Id }, requestBetGameView.GameId, requestBetGameView.BetValue);
			_logger.Log(LogHelper.GetEvent(human.Id, requestBetGameView.GameId, UserMessages.PlayerPlaceBet(requestBetGameView.BetValue)));
			List<long> botsId = await _playerInGameRepository.GetBotsIdByGameId(requestBetGameView.GameId);

			foreach (var botId in botsId)
			{
				_logger.Log(LogHelper.GetEvent(botId, requestBetGameView.GameId, UserMessages.PlayerPlaceBet(Constant.BotsBetValue)));
			}

			await _playerInGameRepository.UpdateBet(botsId, requestBetGameView.GameId, Constant.BotsBetValue);
			List<long> deck = await GetDeckInGame(requestBetGameView.GameId);
			List<long> playersId = await _playerInGameRepository.GetAllPlayersIdByGameId(requestBetGameView.GameId);

			foreach (var playerId in playersId)
			{
				deck = await AddCardToPlayerHand(playerId, deck, requestBetGameView.GameId);
				deck = await AddCardToPlayerHand(playerId, deck, requestBetGameView.GameId);
			}

			GetGameGameView getGameGameView = await GetGame(requestBetGameView.GameId);
			getGameGameView.Options = UserMessages.OptionDrawCard;

			if ((getGameGameView.Human.Hand.CardsInHandValue >= Constant.WinValue)
				|| (getGameGameView.Dealer.Hand.CardsInHandValue >= Constant.WinValue))
			{
				StandGameView standGameView = await Stand(requestBetGameView.GameId);
				getGameGameView = Mapper.Map<StandGameView, GetGameGameView>(standGameView);
			}

			responseBetGameView = Mapper.Map<GetGameGameView, ResponseBetGameView>(getGameGameView);
			return responseBetGameView;
		}

		public async Task<DrawGameView> DrawCard(long gameId)
		{
			
			long humanId = await _playerInGameRepository.GetHumanIdByGameId(gameId);
			List<long> deck = await GetDeckInGame(gameId);
			int humanBetValue = await _playerInGameRepository.GetBetByPlayerId(humanId, gameId);

			if (humanBetValue == 0)
			{
				throw new Exception(UserMessages.NoBetValue);
			}

			deck = await AddCardToPlayerHand(humanId, deck, gameId);
            GetGameGameView getGameView = await GetGame(gameId);
			getGameView.Options = UserMessages.OptionDrawCard;

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
            GetGameGameView getGameView = await GetGame(gameId);
            List<Card> deck = await _cardRepository.GetByIds(getGameView.Deck);

			if (getGameView.Human.BetValue == 0)
			{
				throw new Exception(UserMessages.NoBetValue);
			}

			if ((getGameView.Dealer.Hand.CardsInHandValue != Constant.WinValue)
				|| (getGameView.Dealer.Hand.CardsInHand.Count() != Constant.NumberCardForBlackJack))
			{
				for (var i = 0; i < getGameView.Bots.Count(); i++)
				{
                    HandViewItem botHand = await GetPlayerHand(getGameView.Bots[i].Id, gameId);
					await BotTurn(getGameView.Bots[i].Id, deck, gameId, botHand);
				}
			}

            HandViewItem dealerHand = await GetPlayerHand(getGameView.Dealer.Id, gameId);
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
			getGameView.Options = UserMessages.OptionSetBet(message);
			standGameView = Mapper.Map<GetGameGameView, StandGameView>(getGameView);

			return standGameView;
		}

        private async Task<GetGameGameView> GetGame(long gameId)
        {
            var getGameMapper = new GameMapper();
            Game game = await _gameRepository.GetById(gameId);
            List<long> playerIds = game.PlayersInGame.Select(playerInGame => playerInGame.PlayerId).ToList();
            List<PlayerInGame> playersInGame = await _playerInGameRepository.GetPlayersInGameByPlayerIds(playerIds);
            List<long> deck = await GetDeckInGame(gameId);
            GetGameGameView getGameGameView = getGameMapper.GetView(game, playersInGame, deck);

            if (getGameGameView.Human.Points <= Constant.MinPointsValueToPlay)
            {
                await _playerRepository.UpdatePlayersPoints(new List<long> { getGameGameView.Human.Id }, Constant.DefaultPointsValue);
                getGameGameView.Human.Points = Constant.DefaultPointsValue;
            }

            return getGameGameView;
        }

        private async Task RestoreCardsInDb()
        {
            List<Card> cardsInDb = await _cardRepository.GetAll();

            if (cardsInDb.Count != Constant.DeckSize)
            {
                await _cardRepository.DeleteAll();
                List<Card> cards = GenerateCards();
                await _cardRepository.Add(cards);
            }
        }

        private List<Card> GenerateCards()
        {
            var cardColorValue = 0;
            var cardTitleValue = 0;
            var cardColorSize = Enum.GetNames(typeof(CardSuit)).Length - 1;
            var deck = new List<Card>();
            var valueList = Enumerable.Range(Constant.NumberStartCard, Constant.AmountNumberCard).ToList();
            List<string> titleList = valueList.ConvertAll<string>(delegate (int value)
            {
                return value.ToString();
            });

            foreach (var value in Enum.GetNames(typeof(CardTitle)))
            {
                titleList.Add(value);
            }

            for (var i = 0; i < Enum.GetValues(typeof(CardTitle)).Length - 1; i++)
            {
                valueList.Add(Constant.ImageCardValue);
            }

            valueList.Add(Constant.AceCardValue);

            for (var i = 0; i < Constant.DeckSize; i++)
            {
                var card = new Card
                {
                    Id = i + 1,
                    Title = titleList[cardTitleValue],
                    Value = valueList[cardTitleValue],
                    Suit = (CardSuit)cardColorValue++
                };
                deck.Add(card);

                if (cardColorValue > cardColorSize)
                {
                    cardColorValue = 0;
                    cardTitleValue++;
                }
            }

            return deck;
        }

        private async Task<List<long>> GetDeckInGame(long gameId)
		{
			List<long> cardsInGameId = await _cardInHandRepository.GetCardsIdByGameId(gameId);
			List<long> deck = await LoadInGameDeck(cardsInGameId);

			return deck;
		}

        private async Task<bool> BotTurn(long botId, List<Card> deck, long gameId, HandViewItem hand)
        {
            if(hand.CardsInHandValue >= Constant.ValueToStopDraw)
            {
                await AddMultipleCardsInHand(botId, hand.CardsInHand, gameId);

                return false;
            }

            var card = Mapper.Map<Card, CardViewItem>(deck[0]);
            _logger.Log(LogHelper.GetEvent(botId, gameId, UserMessages.PlayerDrawCard(deck[0].Id)));
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

        private async Task AddMultipleCardsInHand(long playerId, List<CardViewItem> cards, long gameId)
        {
            List<long> cardsInHandId = await _cardInHandRepository.GetCardsIdByPlayerIdAndGameId(playerId, gameId);
            var newCards = new List<long>();
            var CardsInHand = new List<CardInHand>();

            foreach(var card in cards)
            {
                if (!cardsInHandId.Contains(card.Id))
                {
                    newCards.Add(card.Id);
                }
            }
            
            foreach(var newCard in newCards)
            {
                var cardInHand = new CardInHand();
                cardInHand.CardId = newCard;
                cardInHand.PlayerId = playerId;
                cardInHand.GameId = gameId; 

                CardsInHand.Add(cardInHand);
            }

            await _cardInHandRepository.Add(CardsInHand);
        }

		private async Task<HandViewItem> GetPlayerHand(long playerId, long gameId)
		{
			var hand = new HandViewItem
			{
				CardsInHand = new List<CardViewItem>()
			};

			List<long> cardsIdInPlayersHand = await _cardInHandRepository.GetCardsIdByPlayerIdAndGameId(playerId, gameId);
			var cards = await _cardRepository.GetByIds(cardsIdInPlayersHand);
			hand.CardsInHand = Mapper.Map<List<Card>, List<CardViewItem>>(cards);
            hand.CardsInHandValue = CalculateCardsValue(hand.CardsInHand);

			return hand;
		}

        private async Task<List<long>> AddCardToPlayerHand(long playerId, List<long> deck, long gameId)
        {
            await _cardInHandRepository.Add(new CardInHand() { PlayerId = playerId, CardId = deck[0], GameId = gameId });
			_logger.Log(LogHelper.GetEvent(playerId, gameId, UserMessages.PlayerDrawCard(deck[0])));
			deck.Remove(deck[0]);

			return deck;
		}

		private async Task<string> UpdateScore(PlayerViewItem player, int dealerCardsValue, long gameId)
		{
			_logger.Log(LogHelper.GetEvent(player.Id, gameId, UserMessages.PlayerValue(player.Hand.CardsInHandValue, dealerCardsValue)));

			if ((player.Hand.CardsInHandValue > dealerCardsValue)
			&& (player.Hand.CardsInHandValue <= Constant.WinValue))
			{
				await UpdateWonPlayersPoints(player.Id, gameId, player.BetValue, player.Points);
				return UserMessages.OptionWin;
			}

			if ((player.Hand.CardsInHandValue <= Constant.WinValue)
			&& (dealerCardsValue > Constant.WinValue))
			{
				await UpdateWonPlayersPoints(player.Id, gameId, player.BetValue, player.Points);
				return UserMessages.OptionWin;
			}

			if (player.Hand.CardsInHandValue > Constant.WinValue)
			{
				await UpdateLostPlayersPoints(player.Id, gameId, player.BetValue, player.Points);
				return UserMessages.OptionLose;
			}

			if ((dealerCardsValue > player.Hand.CardsInHandValue)
			&& (dealerCardsValue <= Constant.WinValue))
			{
				await UpdateLostPlayersPoints(player.Id, gameId, player.BetValue, player.Points);
				return UserMessages.OptionLose;
			}

			_logger.Log(LogHelper.GetEvent(player.Id, gameId, UserMessages.PlayerDraw));
			return UserMessages.OptionDraw;
		}

		private async Task UpdateLostPlayersPoints(long playerId, long gameId, int playerBetValue, int playerPoints)
		{
			int newPointsValue = playerPoints - playerBetValue;

			_logger.Log(LogHelper.GetEvent(playerId, gameId, UserMessages.PlayerLose(playerBetValue)));
			await _playerRepository.UpdatePlayersPoints(new List<long> { playerId }, newPointsValue);
		}

		private async Task UpdateWonPlayersPoints(long playerId, long gameId, int playerBetValue, int playerPoints)
		{
			int newPointsValue = playerPoints + playerBetValue;

			_logger.Log(LogHelper.GetEvent(playerId, gameId, UserMessages.PlayerWin(playerBetValue)));
			await _playerRepository.UpdatePlayersPoints(new List<long> { playerId }, newPointsValue);
		}

        private async Task<List<long>> LoadInGameDeck(List<long> cardsInGame)
        {
            var deck = new List<long>();
            var randomNumericGenerator = new Random();
            List<Card> cards = (await _cardRepository.GetAll()).ToList();

            foreach (var cardId in cardsInGame)
            {
                cards.RemoveAll(c => c.Id == cardId);
            }

            foreach (var card in cards)
            {
                deck.Add(card.Id);
            }

            for (var i = 0; i < deck.Count(); i++)
            {
                var index = randomNumericGenerator.Next(deck.Count());
                var value = deck[i];
                deck[i] = deck[index];
                deck[index] = value;
            }

            return deck;
        }
    }
}