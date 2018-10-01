using AutoMapper;
using BlackJack.BusinessLogic.Helpers;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.Configurations;
using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;
using BlackJack.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Mappers
{
	public class GameMapper
	{
		private ICardProvider _cardProvider;

		private IPlayerInGameRepository _playerInGameRepository;
		private IGameRepository _gameRepository;
		private IHandRepository _handRepository;
		private IPlayerRepository _playerRepository;

		public GameMapper(ICardProvider cardProvider, IHandRepository handRepository, IPlayerRepository playerRepository, IPlayerInGameRepository playerInGameRepository, IGameRepository gameRepository)
		{
			_handRepository = handRepository;
			_playerRepository = playerRepository;
			_playerInGameRepository = playerInGameRepository;
			_gameRepository = gameRepository;
			_cardProvider = cardProvider;
		}

		public async Task<GetGameViewModel> GetGameViewModelByGameId(long gameId)
		{
			var getGameViewModel = new GetGameViewModel();
			Game game = await _gameRepository.GetGameById(gameId);
			var human = game.PlayersInGame.Where(player => player.IsHuman == true).FirstOrDefault();

			getGameViewModel.Human = Mapper.Map<Player, PlayerViewModelItem>(human.Player);
			getGameViewModel.Human.BetValue = human.BetValue;

			if (getGameViewModel.Human.Points <= Constant.MinPointsValueToPlay)
			{
				await _playerRepository.RestorePlayerPoints(getGameViewModel.Human.Id);
				getGameViewModel.Human.Points = Constant.DefaultPointsValue;
			}

			getGameViewModel.Human.Hand = await GetPlayerHand(getGameViewModel.Human.Id, gameId);

			getGameViewModel.Dealer = Mapper.Map<Player, DealerViewModelItem>(await _playerRepository.GetDealerByGameId(gameId));
			getGameViewModel.Dealer.Hand = await GetPlayerHand(getGameViewModel.Dealer.Id, game.Id);
			getGameViewModel.Deck = await GetInGameDeck(gameId);
			getGameViewModel.Bots = new List<PlayerViewModelItem>();

			List<long> botsId = await _playerInGameRepository.GetBotsIdByGameId(game.Id);
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
				getGameViewModel.Options = StringHelper.OptionDrawCard;
			}

			if ((getGameViewModel.Human.Hand.CardsInHand.Count() == 0)
				|| (getGameViewModel.Human.BetValue == 0))
			{
				getGameViewModel.Options = StringHelper.OptionSetBet(string.Empty);
			}

			return getGameViewModel;
		}

		private async Task<HandViewModelItem> GetPlayerHand(int playerId, long gameId)
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

		private async Task<List<long>> GetInGameDeck(long gameId)
		{
			List<long> cardsInGameId = await _handRepository.GetCardsIdByGameId(gameId);
			var deck = await _cardProvider.LoadInGameDeck(cardsInGameId);
			return deck;
		}
	}
}
