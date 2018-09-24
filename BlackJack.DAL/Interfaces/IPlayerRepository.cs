using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.Entities;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IPlayerRepository
    {
        Task<Player> GetPlayerByName(string name);
        Task<Player> GetPlayerById(int id);
		Task<List<Player>> GetPlayersByIds(List<int> idList);
        Task<List<Player>> GetBots(string name, int botsAmount);
        Task CreateNewPlayer(Player player);
        Task UpdatePlayerPoints(int playerId, int newPointsValue);
        Task RestorePlayerPoints(int playerId);
		Task RestorePlayersPoints(List<int> playersId);

	}
}
