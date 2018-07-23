using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.BLL.Services
{
    public static class PointService
    {
        public static GameViewModel WinPoints(GameViewModel gameModel, int playerId)
        {
            gameModel.Players.First(p => p.Id == playerId).Points += gameModel.Players.First(p => p.Id == playerId).Hand.Points;
            gameModel = AnnulPoints(gameModel, playerId);
            return gameModel;
        }

        public static GameViewModel AnnulPoints(GameViewModel gameModel, int playerId)
        {
            gameModel.Players.First(p => p.Id == playerId).Hand.Points = 0;
            return gameModel;
        }

        public static GameViewModel LosePoints(GameViewModel gameModel, int playerId)
        {
            gameModel.Players.First(p => p.Id == playerId).Points -= gameModel.Players.First(p => p.Id == playerId).Hand.Points;
            var zp = gameModel.Players.First(p => p.Id == playerId).Points;
            gameModel = AnnulPoints(gameModel, playerId);
            return gameModel;
        }
    }
}
