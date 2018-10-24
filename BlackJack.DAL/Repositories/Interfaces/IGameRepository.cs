using BlackJack.Entities;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IGameRepository  : IBaseRepository<Game>
	{
		Task DeleteGameById(long gameId);
		Task<long> GetGameIdByHumanId(long humanId);
	}
}
