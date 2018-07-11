using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Services.Interface;
using BlackJack.DAL.Interface;
using BlackJack.ViewModel;
using BlackJack.BLL.Infrastructure;
using BlackJack.Entity;
using BlackJack.Configuration.Constant;

namespace BlackJack.BLL.Services
{
    public class Play : IPlay
    {
        IUnitOfWork DataBase { get; set; }

        public Play(IUnitOfWork unitOfWork)
        {
            DataBase = unitOfWork;
        }

        public void TakeCard(PlayerModel playerModel, List<Card> deck)
        {
            var player = DataBase.Players.Get(playerModel.Id);

            if (player == null)
            {
                throw new ValidationException("Player not found", "");
            }

            var card = DataBase.Cards.Get(deck[0].Id);

            if (player == null)
            {
                throw new ValidationException("Card not found", "");
            }

            deck.Remove(deck[0]);
            var hand = new Hand
            {
                IdCard = card.Id,
                IdPlayer = player.Id
            };

            DataBase.Hands.Create(hand);
            DataBase.Save();
        }

        public int GetCardValue(PlayerModel playerModel)
        {
            int cardsValue = 0;
            var player = DataBase.Players.Get(playerModel.Id);

            if (player == null)
            {
                throw new ValidationException("Player not found", "");
            }

            var cards = DataBase.Hands.GetAll().Where(x => x.IdPlayer == player.Id);

            foreach (var card in cards)
            {
                cardsValue += DataBase.Cards.Get(card.IdCard).Value;
            }

            foreach (var card in cards)
            {
                if ((DataBase.Cards.Get(card.IdCard).Title == Constant.NameCardForBlackJack) && (cardsValue > Constant.WinValue))
                {
                    cardsValue -= Constant.ImageCardValue;
                }
            }

            if (CombinationChecker.PlayerHandCardListIsBlackJack(playerModel))
            {
                cardsValue = Constant.WinValue;
            }

            return cardsValue;
        }

        public List<CardModel> GetCardsInHand(PlayerModel playerModel)
        {
            var cardList = new List<CardModel>();
            
            return cardList;
        }

    }
}
