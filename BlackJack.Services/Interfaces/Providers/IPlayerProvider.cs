using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.Entities;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IPlayerProvider
    {
		Task<List<Player>> GetBotsInfo(IEnumerable<int> botsIdList);
		Task<int> GetIdByName(string name);
		Task<Player> GetPlayerInfo(int playerId);
		Task<Player> GetDealer(int gameId);
		Task<string> UpdateScore(int playerId, int playerBetValue, int playerCardsValue, int dealerCardsValue, int gameId);

	}
}
