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
        public static GameModel WinPoints(GameModel gameModel, int playerId)
        {
            gameModel.Players.First(p => p.Id == playerId).Points += gameModel.Players.First(p => p.Id == playerId).Hand.Points;
            gameModel = AnnulPoints(gameModel, playerId);
            return gameModel;
        }

        public static GameModel AnnulPoints(GameModel gameModel, int playerId)
        {
            gameModel.Players.First(p => p.Id == playerId).Hand.Points = 0;
            return gameModel;
        }

        public static GameModel LosePoints(GameModel gameModel, int playerId)
        {
            gameModel.Players.First(p => p.Id == playerId).Points -= gameModel.Players.First(p => p.Id == playerId).Hand.Points;
            gameModel = AnnulPoints(gameModel, playerId);
            return gameModel;
        }
    }
}
