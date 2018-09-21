using BlackJack.Entities;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IGameRepository
	{
		Task<int> Create();
		Task Delete(int id);
		Task<int> GetGameIdByHumanId(int humanId);
		Task<Game> GetGameById(int id);
	}
}
