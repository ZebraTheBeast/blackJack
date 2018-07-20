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
            gameModel.GameStat.Add($"Player {gameModel.Players.Find(p => p.Id == playerId).Name} bet {gameModel.Players.Find(p => p.Id == playerId).Hand.Points} points.");
            return gameModel;
        }

        public static GameModel PlayerDrawCard(GameModel gameModel, int playerIndex)
        {
            gameModel.GameStat.Add($"Player {gameModel.Players[playerIndex].Name} draw card {gameModel.Deck[0].Title} of {gameModel.Deck[0].Color}.");
            return gameModel;
        }

        public static GameModel PlayerWon(GameModel gameModel, string playerName)
        {
            gameModel.GameStat.Add($"Player {playerName} has won.");
            return gameModel;
        }

        public static GameModel PlayerLost(GameModel gameModel, string playerName)
        {
            gameModel.GameStat.Add($"Player {playerName} has lost.");
            return gameModel;
        }

        public static GameModel PlayerDraw(GameModel gameModel, string playerName)
        {
            gameModel.GameStat.Add($"Player {playerName} has draw with Dealer.");
            return gameModel;
        }

        public static GameModel NewGame(GameModel gameModel)
        {
            gameModel.GameStat.Add("New game!");
            return gameModel;
        }
    }
}
