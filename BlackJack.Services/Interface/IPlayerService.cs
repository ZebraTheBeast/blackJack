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
        Task<List<PlayerViewModel>> GetBotsInGame();
        Task SetPlayerToGame(string playerName);
        Task<IEnumerable<int>> GetPlayersIdInGame();
        Task PlaceBet(int playerId, int betValue);
        Task<PlayerViewModel> GetHumanInGame();
        Task<DealerViewModel> GetDealer();
    }
}
