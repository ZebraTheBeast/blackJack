using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.BLL.Interface
{
    public interface IGame
    {
        GameModel Dealing(GameModel gameModel);
        GameModel GiveCard(int playerId, GameModel gameModel);
        GameModel PlayerTest(GameModel gameModel, PlayerModel player);
        GameModel BotTurn(GameModel gameModel, PlayerModel player, int minValue);
        GameModel EditPoints(GameModel gameModel);
        GameModel EndTurn(GameModel gameModel);
        GameModel PlaceBet(GameModel gameModel, int playerId, int pointsValue);
    }
}
