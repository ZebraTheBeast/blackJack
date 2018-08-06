using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Configuration.Constant;
using BlackJack.Entity.Enum;
using BlackJack.Entity;
using BlackJack.DAL.Interface;
using BlackJack.DAL.Repository;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Helper;
using NLog;
using System.IO;

namespace BlackJack.BLL.Services
{
    public class DeckService : IDeckService
    {
        IHandRepository _handRepository;
        IPlayerRepository _playerRepository;
        IPlayerInGameRepository _playerInGameRepository;

        public DeckService(IHandRepository handRepository, IPlayerRepository playerRepository, IPlayerInGameRepository playerInGameRepository)
        {
            _handRepository = handRepository;
            _playerRepository = playerRepository;
            _playerInGameRepository = playerInGameRepository;
        }

        public List<int> GetNewRefreshedDeck()
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            var cardViewModelList = new List<CardViewModel>();
            var deck = new List<int>();

            cardViewModelList = CardHelper.GetFullDeck();
            foreach (var card in cardViewModelList)
            {
                deck.Add(card.Id);
            }

            deck = ShuffleDeck(deck);
            logger.Info(StringHelper.DeckShuffled());
            return deck;
        }

        public async Task<List<int>> LoadDeck(int gameId)
        {
            var cardViewModelList = new List<CardViewModel>();
            var deck = new List<int>();
            var cardIdList = new List<int>();
            cardViewModelList = CardHelper.GetFullDeck();
            var players = await _playerInGameRepository.GetAll(gameId);
            foreach (var playerId in players)
            {
                cardIdList.AddRange(await _handRepository.GetIdCardsByPlayerId(playerId, gameId));
            }

            foreach(var cardId in cardIdList)
            {
                cardViewModelList.RemoveAll(c => c.Id == cardId);
            }

            foreach(var card in cardViewModelList)
            {
                deck.Add(card.Id);
            }

            deck = ShuffleDeck(deck);

            return deck;
        }

        private List<int> ShuffleDeck(List<int> deck)
        {
            Random rng = new Random();
            int deckSize = deck.Count();

            for (var i = 0; i < deckSize; i++)
            {
                int index = rng.Next(deckSize);
                var value = deck[i];
                deck[i] = deck[index];
                deck[index] = value;
            }
              
            return deck;
        }

        public async Task GiveCardFromDeck(int playerId, int cardId, int gameId)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                if (!await _playerInGameRepository.IsInGame(playerId, gameId))
                {
                    throw new Exception(StringHelper.PlayerNotInGame());
                }

                await _handRepository.GiveCardToPlayer(playerId, cardId, gameId);
                logger.Info(StringHelper.PlayerDrawCard(playerId,cardId));
            }
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                throw exception;
            }
        }
    }
}
