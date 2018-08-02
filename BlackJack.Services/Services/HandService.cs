using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Helper;
using BlackJack.ViewModel;
using BlackJack.DAL.Interface;
using BlackJack.Configuration.Constant;
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

        public async Task<HandViewModel> GetPlayerHand(int playerId)
        {
            try
            {
                var hand = new HandViewModel
                {
                    CardList = new List<CardViewModel>()
                };

                var player = _playerInGameRepository.GetBetByPlayerId(playerId);

                if(player == null)
                {
                    throw new Exception(StringHelper.PlayerNotInGame());
                }

                var cardsId = await _handRepository.GetIdCardsByPlayerId(playerId);

                foreach (var cardId in cardsId)
                {
                    var card = CardHelper.GetCardById(cardId);
                    hand.CardList.Add(card);
                }

                hand.Points = await _playerInGameRepository.GetBetByPlayerId(playerId);
                hand.CardListValue = CountPlayerCardsValue(hand.CardList);

                return hand;
            }

            catch(Exception exception)
            {
                var logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Error($"{exception.Message}");
                throw exception;
            }
        }

        public async Task<int> GetPlayerHandValue(int playerId)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                var player = _playerInGameRepository.GetBetByPlayerId(playerId);

                if (player == null)
                {
                    throw new Exception(StringHelper.PlayerNotInGame());
                }

                var cardIdList = await _handRepository.GetIdCardsByPlayerId(playerId);
                var cards = new List<CardViewModel>();

                foreach (var cardId in cardIdList)
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
                if ((card.Title == Constant.NameCardForBlackJack) && (cardListValue > Constant.WinValue))
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

        public async Task RemoveAllCardsInHand()
        {
            await _handRepository.RemoveAll();
        }
    }
}
