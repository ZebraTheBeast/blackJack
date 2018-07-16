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
        GameModel GiveCard(PlayerModel player, GameModel gameModel);
        GameModel AddPlayers(string name);
        GameModel PlayerTest(GameModel gameModel, PlayerModel player);
        GameModel PullCard(int id);
    }
}
