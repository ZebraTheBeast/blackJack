using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.Entities;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task<Player> GetPlayerByName(string name);
		Task<List<Player>> GetPlayersByIds(List<long> idList);
        Task<List<Player>> GetBotsWithDealer(string name, int botsAmount);
        Task UpdatePlayerPoints(long playerId, int newPointsValue);
        Task RestorePlayerPoints(long playerId);
		Task RestorePlayersPoints(List<long> playersId);
	}
}
