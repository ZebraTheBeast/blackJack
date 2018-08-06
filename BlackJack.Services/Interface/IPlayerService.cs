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
        Task<List<PlayerViewModel>> GetBotsInGame(int gameId);
        Task<int> SetPlayerToGame(string playerName);
        Task<IEnumerable<int>> GetPlayersIdInGame(int gameId);
        Task PlaceBet(int playerId, int betValue, int gameId);
        Task<PlayerViewModel> GetHumanInGame(int gameId);
        Task<DealerViewModel> GetDealer(int gameId);
        Task<int> GetIdByName(string name);
    }
}
