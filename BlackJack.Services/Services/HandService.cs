using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Helper;
using BlackJack.ViewModel;
using BlackJack.DAL.Interface;
using BlackJack.Configuration;
using System.IO;

namespace BlackJack.BLL.Services
{
    public class HandService : IHandService
    {
        IHandRepository _handRepository;
        IPlayerInGameRepository _playerInGameRepository;

        public HandService(IHandRepository handRepository, IPlayerInGameRepository playerInGameRepository)
        {
            _handRepository = handRepository;
            _playerInGameRepository = playerInGameRepository;
        }

        public async Task<HandViewModel> GetPlayerHand(int playerId, int gameId)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                var hand = new HandViewModel
                {
                    CardList = new List<CardViewModel>()
                };

                if( !await _playerInGameRepository.IsInGame(playerId, gameId))
                {
                    throw new Exception(StringHelper.PlayerNotInGame());
                }

                var cardsIdList = await _handRepository.GetIdCardsByPlayerId(playerId, gameId);

                foreach (var cardId in cardsIdList)
                {
                    var card = CardHelper.GetCardById(cardId);
                    hand.CardList.Add(card);
                }

                hand.BetValue = await _playerInGameRepository.GetBetByPlayerId(playerId, gameId);
                hand.CardListValue = CountPlayerCardsValue(hand.CardList);

                return hand;
            }

            catch(Exception exception)
            {
                logger.Error(exception.Message);
                throw exception;
            }
        }

        public async Task<int> GetPlayerHandValue(int playerId, int gameId)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                if ( !await _playerInGameRepository.IsInGame(playerId, gameId))
                {
                    throw new Exception(StringHelper.PlayerNotInGame());
                }

                var playerCardsIdList = await _handRepository.GetIdCardsByPlayerId(playerId, gameId);
                var cards = new List<CardViewModel>();

                foreach (var cardId in playerCardsIdList)
                {
                    var card = CardHelper.GetCardById(cardId);
                    cards.Add(card);
                }

                var handValue = CountPlayerCardsValue(cards);

                return handValue;
            }
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                throw exception;
            }
        }

        private int CountPlayerCardsValue(List<CardViewModel> cards)
        {
            var cardListValue = 0;

            foreach (var card in cards)
            {
                cardListValue += card.Value;
            }

            foreach (var card in cards)
            {
                if ((card.Title == Constant.NameCardForBlackJack) 
                    && (cardListValue > Constant.WinValue))
                {
                    cardListValue -= Constant.ImageCardValue;
                }
            }

            if(cards.Count() != Constant.NumberCardForBlackJack)
            {
                return cardListValue;
            }

            foreach(var card in cards)
            {
                if(card.Title != Constant.NameCardForBlackJack)
                {
                    return cardListValue;
                }
            }

            cardListValue = Constant.WinValue;
            return cardListValue;
        }

        public async Task RemoveAllCardsInHand(int gameId)
        {
            await _handRepository.RemoveAll(gameId);
        }
    }
}
