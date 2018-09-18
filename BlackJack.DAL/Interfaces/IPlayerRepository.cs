using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.Entities;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IPlayerRepository
    {
        Task<Player> GetByName(string name);
        Task<Player> GetById(int id);
		Task<List<Player>> GetPlayers(List<int> idList);
        Task<List<Player>> GetBots(string name, int botsAmount);
        Task Create(Player player);
        Task UpdatePoints(int playerId, int newPointsValue);
        Task RestorePoints(int playerId);
    }
}
