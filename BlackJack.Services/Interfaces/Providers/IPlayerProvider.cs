using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.ViewModels;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IPlayerProvider
    {
		Task<List<PlayerViewModel>> GetBotsInfo(IEnumerable<int> botsIdList);
		Task<int> GetIdByName(string name);
		Task<PlayerViewModel> GetPlayerInfo(int playerId);
		Task<DealerViewModel> GetDealer(int gameId);
		Task<string> UpdateScore(int playerId, int playerBetValue, int playerCardsValue, int dealerCardsValue, int gameId);

	}
}
