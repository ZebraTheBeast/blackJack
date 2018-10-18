using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.Entities;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task<Player> GetPlayerByName(string name);
        Task<Player> GetDealer();
        Task<List<Player>> GetBots(int botsAmount);
		Task UpdatePlayersPoints(List<long> playersId, int newPointsValue);
	}
}
