using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
		Task<TEntity> GetByIdAsync(long id);
	}
}
