using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
namespace BlackJack.BLL.Interface
{
    public interface IPlayerService
    {
        void PlayerAdd(string playerName);
        List<PlayerViewModel> GetPlayers();
        
    }
}
