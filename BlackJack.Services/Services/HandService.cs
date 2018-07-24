using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.ViewModel;
using BlackJack.DAL.Interface;
using BlackJack.Configuration.Constant;

namespace BlackJack.BLL.Services
{
    public class HandService : IHandService
    {
        IHandRepository _handRepository;
        ICardRepository _cardRepository;
        IPlayerInGameRepository _playerInGameRepository;

        public HandService(IHandRepository handRepository, ICardRepository cardRepository, IPlayerInGameRepository playerInGameRepository)
        {
            _handRepository = handRepository;
            _cardRepository = cardRepository;
            _playerInGameRepository = playerInGameRepository;
        }

        public HandViewModel GetPlayerHand(int playerId)
        {
            var hand = new HandViewModel { CardList = new List<CardViewModel>() };

            var cardsId = _handRepository.GetIdCardsByPlayerId(playerId);

            foreach (var cardId in cardsId)
            {
                var card = _cardRepository.GetById(cardId);

                var cardViewModel = new CardViewModel();
                cardViewModel.Id = card.Id;
                cardViewModel.Title = card.Title;
                cardViewModel.Color = card.Color.ToString();
                cardViewModel.Value = card.Value;

                hand.CardList.Add(cardViewModel);
            }

            hand.Points = _playerInGameRepository.GetBetByPlayerId(playerId);
            hand.CardListValue = CountPlayerCardsValue(hand.CardList);

            return hand;
        }

        public int GetPlayerHandValue(int playerId)
        {
            var cardsId = _handRepository.GetIdCardsByPlayerId(playerId);
            var cards = new List<CardViewModel>();

            foreach (var cardId in cardsId)
            {
                var card = _cardRepository.GetById(cardId);
                var cardViewModel = new CardViewModel();
                cardViewModel.Id = card.Id;
                cardViewModel.Title = card.Title;
                cardViewModel.Color = card.Color.ToString();
                cardViewModel.Value = card.Value;
                cards.Add(cardViewModel);
            }

            var handValue = CountPlayerCardsValue(cards);

            return handValue;
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

            return cardListValue;
        }

        public void RemoveAllCardsInHand()
        {
            _handRepository.RemoveAll();
        }
    }
}
