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
        List<PlayerViewModel> GetBotsInGame();
        PlayerViewModel GetHumanInGame();
        DealerViewModel GetDealer();
        void SetPlayerToGame(string playerName);   
        List<int> GetPlayersIdInGame();
        void RemoveAllPlayers();
        void MakeBet(int playerId, int betValue);
    }
}
