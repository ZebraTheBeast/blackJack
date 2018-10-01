using BlackJack.Entities;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IGameRepository
	{
		Task<long> StartNewGame();
		Task DeleteGameById(long gameId);
		Task<long> GetGameIdByHumanId(long humanId);
		Task<Game> GetGameById(long gameId);
	}
}
