using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IScoreProvider
    {
        Task<string> UpdateScore(int playerId, int playerCardsValue, int dealerCardsValue, int gameId);
    }
}
