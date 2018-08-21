using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IScoreService
    {
        Task<string> UpdateScore(int playerId, int playerCardsValue, int dealerCardsValue, int gameId);
    }
}
