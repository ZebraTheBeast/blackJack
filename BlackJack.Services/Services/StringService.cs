using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.BLL.Services
{
    public static class StringService
    {
        public static GameModel PlayerBetPoint(GameModel gameModel, int playerId)
        {
            gameModel.GameStats.Add($"{gameModel.Players.Find(p => p.Id == playerId).Name} bet {gameModel.Players.Find(p => p.Id == playerId).Hand.Points} points.");
            return gameModel;
        }

        public static GameModel PlayerDrawCard(GameModel gameModel, int playerIndex)
        {
            gameModel.GameStats.Add($"{gameModel.Players[playerIndex].Name} draw card {gameModel.Deck[0].Title} of {gameModel.Deck[0].Color}.");
            return gameModel;
        }

        public static GameModel PlayerWon(GameModel gameModel, string playerName)
        {
            gameModel.GameStats.Add($"{playerName} has won.");
            gameModel = OptionService.OptionWin(gameModel);
            return gameModel;
        }

        public static GameModel PlayerLose(GameModel gameModel, string playerName)
        {
            gameModel.GameStats.Add($"{playerName} has lose.");
            gameModel = OptionService.OptionLose(gameModel);
            return gameModel;
        }

        public static GameModel PlayerDraw(GameModel gameModel, string playerName)
        {
            gameModel.GameStats.Add($"{playerName} has draw with Dealer.");
            gameModel = OptionService.OptionDraw(gameModel);
            return gameModel;
        }
    }
}
