using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.DAL.Interface;
using BlackJack.ViewModel;
using BlackJack.BLL.Infrastructure;
using BlackJack.Entity;
using BlackJack.Configuration.Constant;
using AutoMapper;

namespace BlackJack.BLL.Services
{
    public class PlayService : IPlay
    {
        // TODO - s bd ne rabotat, a s repositoriyami
        IUnitOfWork DataBase { get; set; }

        public PlayService(IUnitOfWork unitOfWork)
        {
            DataBase = unitOfWork;
        }

        public int GetCardValue(PlayerModel playerModel)
        {
            int cardsValue = 0;
            var player = DataBase.Players.Get(playerModel.Id);

            if (player == null)
            {
                // TODO - v helper zasunut stringHelper, shob izmenit "Player not found" i ne ebatsa
                throw new ValidationException("Player not found");
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

            if (CombinationCheckerService.PlayerHandCardListIsBlackJack(playerModel))
            {
                cardsValue = Constant.WinValue;
            }

            return cardsValue;
        }

        public List<CardModel> GetCardsInHand(PlayerModel playerModel)
        {
            var cardIdList = DataBase.Hands.GetAll().Where(x => x.IdPlayer == playerModel.Id);

            if (cardIdList == null)
            {
                throw new ValidationException("Player hasn't card in hand");
            }

            var cardList = new List<CardModel>();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Card, CardModel>()).CreateMapper();
            
            foreach(var card in cardIdList)
            {
                cardList.Add(mapper.Map<Card, CardModel>(DataBase.Cards.Get(card.IdCard)));
            }
            return cardList;
        }

        public void PutPoints(PlayerModel playerModel, int pointsValue)
        {
            var player = DataBase.Players.Get(playerModel.Id);

            if (player == null)
            {
                throw new ValidationException("Player not found");
            }

            if (player.Points >= pointsValue)
            {
                playerModel.Hand.Points = pointsValue;
            }
        }

        public void EmptyHand(PlayerModel playerModel)
        {
            var player = DataBase.Players.Get(playerModel.Id);

            if (player == null)
            {
                throw new ValidationException("Player not found");
            }

            DataBase.Hands.DeleteByPlayerId(playerModel.Id);
            DataBase.Save();
        }

        public void TakeCard(PlayerModel playerModel, List<CardModel> deck)
        {
            var player = DataBase.Players.Get(playerModel.Id);

            if (player == null)
            {
                throw new ValidationException("Player not found");
            }

            var card = DataBase.Cards.Get(deck[0].Id);

            if (player == null)
            {
                throw new ValidationException("Card not found");
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
    }
}
