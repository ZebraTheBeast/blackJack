using BlackJack.BusinessLogic.Helpers;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.Configurations;
using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;
using BlackJack.Entities.Enums;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Services
{
	public class LoginService : ILoginService
	{
		private IPlayerInGameRepository _playerInGameRepository;
		private IPlayerRepository _playerRepository;
		private ICardInHandRepository _cardInHandRepository;
		private IGameRepository _gameRepository;
        private ICardRepository _cardRepository;
		private Logger _logger;

		public LoginService(ICardRepository cardRepository, IPlayerInGameRepository playerInGameRepository, IPlayerRepository playerRepository, ICardInHandRepository handRepository, IGameRepository gameRepository)
		{
			_playerInGameRepository = playerInGameRepository;
			_playerRepository = playerRepository;
			_cardInHandRepository = handRepository;
			_gameRepository = gameRepository;
            _cardRepository = cardRepository;
			_logger = LogManager.GetCurrentClassLogger();
		}

		public async Task<long> StartGame(string playerName, int botsAmount)
		{
			Player human = await _playerRepository.GetPlayerByName(playerName);
            var playersInGame = new List<PlayerInGame>();

			if (human == null)
			{
				var player = new Player { Name = playerName, Type = Entities.Enums.PlayerType.Human };
				await _playerRepository.Add(player);
				human = await _playerRepository.GetPlayerByName(playerName);
			}

			if (human.Points <= Constant.MinPointsValueToPlay)
			{
				await _playerRepository.UpdatePlayersPoints(new List<long> { human.Id }, Constant.DefaultPointsValue);
				human.Points = Constant.DefaultPointsValue;
			}

			List<Player> bots = await _playerRepository.GetBots(botsAmount);
            var dealer = await _playerRepository.GetDealer();
            bots.Add(dealer);
			var oldGameId = await _gameRepository.GetGameIdByHumanId(human.Id);
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

			return gameId;
		}

		public async Task<long> LoadGame(string playerName)
		{
			Player player = await _playerRepository.GetPlayerByName(playerName);

			if (player == null)
			{
				throw new Exception(UserMessages.NoLastGame);
			}

			if (player.Points <= Constant.MinPointsValueToPlay)
			{
				await _playerRepository.UpdatePlayersPoints(new List<long> { player.Id }, Constant.DefaultPointsValue);
				player.Points = Constant.DefaultPointsValue;
			}

			var gameId = await _gameRepository.GetGameIdByHumanId(player.Id);
			await RestoreCardsInDb();

			if (gameId == 0)
			{
				throw new Exception(UserMessages.NoLastGame);
			}

			_logger.Log(LogHelper.GetEvent(player.Id, gameId, UserMessages.PlayerContinueGame));
			return gameId;
		}

        private async Task RestoreCardsInDb()
        {
            var cardsInDb = await _cardRepository.GetAll();

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

    }
}
