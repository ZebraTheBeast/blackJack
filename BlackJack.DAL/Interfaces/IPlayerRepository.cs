using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.Entities;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IPlayerRepository
    {
        Task<Player> GetByName(string name);
        Task<Player> GetById(int id);
        Task<IEnumerable<Player>> GetBots(string name, int botsAmount);
        Task<Player> Create(Player player);
        Task UpdatePoints(int playerId, int newPointsValue);
        Task RestorePoints(int playerId);
    }
}
