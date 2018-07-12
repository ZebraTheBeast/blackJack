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
        void AddPlayer(PlayerModel player);
        void Dealing();
        LoginPlayersModel Test();
    }
}
