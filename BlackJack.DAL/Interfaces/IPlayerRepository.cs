using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.Entities;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IPlayerRepository
    {
        Task<Player> GetPlayerByName(string name);
        Task<Player> GetPlayerById(long playerId);
		Task<List<Player>> GetPlayersByIds(List<long> idList);
        Task<List<Player>> GetBotsWithDealer(string name, int botsAmount);
        Task CreateNewPlayer(Player player);
        Task UpdatePlayerPoints(long playerId, int newPointsValue);
        Task RestorePlayerPoints(long playerId);
		Task RestorePlayersPoints(List<long> playersId);
	}
}
