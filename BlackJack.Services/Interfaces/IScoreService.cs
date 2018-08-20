using System.Threading.Tasks;

namespace BlackJack.BLL.Interfaces
{
	public interface IScoreService
    {
        Task<string> UpdateScore(int playerId, int playerCardsValue, int dealerCardsValue, int gameId);
    }
}
