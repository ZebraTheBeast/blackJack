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
        GameModel Dealing();
        LoginPlayersModel GetStartPlayers();
        void GiveCard(PlayerModel player);
        GameModel AddPlayers(string name);
        GameModel PlayerTest();
        GameModel PullCard(int id);
    }
}
