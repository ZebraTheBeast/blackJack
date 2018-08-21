using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.ViewModels;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IPlayerProvider
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
