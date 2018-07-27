using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.ViewModel;
using BlackJack.DAL.Interface;
using BlackJack.Configuration.Constant;
using System.IO;

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

            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\"));
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(path + "BlackJack.Configuration\\Nlog.config", true);
        }

        public async Task<HandViewModel> GetPlayerHand(int playerId)
        {
            try
            {
                var player = _playerInGameRepository.GetBetByPlayerId(playerId);
                if(player == null)
                {
                    throw new Exception("Player not in game");
                }

                var hand = new HandViewModel { CardList = new List<CardViewModel>() };

                var cardsId = await _handRepository.GetIdCardsByPlayerId(playerId);

                foreach (var cardId in cardsId)
                {
                    var card = await _cardRepository.GetById(cardId);

                    var cardViewModel = new CardViewModel();
                    cardViewModel.Id = card.Id;
                    cardViewModel.Title = card.Title;
                    cardViewModel.Color = card.Color.ToString();
                    cardViewModel.Value = card.Value;

                    hand.CardList.Add(cardViewModel);
                }

                hand.Points = await _playerInGameRepository.GetBetByPlayerId(playerId);
                hand.CardListValue = CountPlayerCardsValue(hand.CardList);

                return hand;
            }
            catch(Exception exception)
            {
                var logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Error($"{exception.Message}");
            }
            return null;
        }

        public async Task<int> GetPlayerHandValue(int playerId)
        {
            try
            {
                var player = _playerInGameRepository.GetBetByPlayerId(playerId);

                if (player == null)
                {
                    throw new Exception("Player not in game");
                }

                var cardsId = await _handRepository.GetIdCardsByPlayerId(playerId);
                var cards = new List<CardViewModel>();

                foreach (var cardId in cardsId)
                {
                    var card = await _cardRepository.GetById(cardId);
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
            catch (Exception exception)
            {
                var logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Error($"{exception.Message}");
            }
            return 0;
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
