using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.Entities;
using BlackJack.Entities.Enums;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IPlayerRepository : IBaseRepository<Player>
    {
        Task<Player> GetPlayerByName(string name);
        Task<List<Player>> GetByAmountAndType(int botsAmount, PlayerType playerType);
		Task UpdatePlayersPoints(List<long> playersId, int newPointsValue);
	}
}
