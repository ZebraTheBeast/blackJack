using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IGameRepository
	{
		Task Create(int humanId);
		Task<int> GetGameIdByHumanId(int humanId);
		Task Delete(int id);
	}
}
