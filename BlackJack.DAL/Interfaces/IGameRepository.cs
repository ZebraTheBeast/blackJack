using BlackJack.Entities;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IGameRepository
	{
		Task Create(int humanId);
		Task Delete(int id);
		Task<int> GetGameIdByHumanId(int humanId);
		Task<Game> GetGameById(int id);
		Task<int> GetHumanIdByGameId(int gameId);
	}
}
