using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.BLL.Interface;
using BlackJack.DAL.Interface;
using BlackJack.BLL.Infrastructure;

namespace BlackJack.BLL.Services
{
    public class Point : IPoint
    {
        IUnitOfWork DataBase { get; set; }

        public Point(IUnitOfWork unitOfWork)
        {
            DataBase = unitOfWork;
        }

        public void LosePoints(PlayerModel playerModel)
        {
            var player = DataBase.Players.Get(playerModel.Id);

            if (player == null)
            {
                throw new ValidationException("Player not found");
            }

            player.Points -= playerModel.Hand.Points;
            playerModel.Hand.Points = 0;
            DataBase.Players.Update(player);
            DataBase.Save();
        }

        public void WinPoints(PlayerModel playerModel)
        {
            var player = DataBase.Players.Get(playerModel.Id);

            if (player == null)
            {
                throw new ValidationException("Player not found");
            }

            player.Points += playerModel.Hand.Points;
            playerModel.Hand.Points = 0;
            DataBase.Players.Update(player);
            DataBase.Save();
        }

        public void AnnulPoints(PlayerModel playerModel)
        {
            playerModel.Hand.CardListValue = 0;
        }
    }
}
