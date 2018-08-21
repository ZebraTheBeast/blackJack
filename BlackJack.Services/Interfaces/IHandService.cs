using System.Threading.Tasks;
using BlackJack.ViewModels;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IHandService
    {
        Task<HandViewModel> GetPlayerHand(int playerId, int gameId);
        Task<int> GetPlayerHandValue(int playerId, int gameId);
        Task RemoveAllCardsInHand(int gameId);
    }
}
