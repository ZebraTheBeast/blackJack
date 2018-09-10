using BlackJack.Entities;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IGameRepository
	{
		Task Create(int humanId);
		Task Delete(int id);
		Task<Game> GetGameByHumanId(int humanId);
	}
}
