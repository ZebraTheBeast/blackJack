using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IDeckService
    {
        List<int> GetNewRefreshedDeck();
        Task<List<int>> LoadDeck(int gameId);
        Task GiveCardFromDeck(int playerId, int cardId, int gameId);
    }
}
