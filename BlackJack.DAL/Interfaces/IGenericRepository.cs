using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
		Task<TEntity> GetById(long id);
		Task<List<TEntity>> GetAll();
		Task DeleteAll();
		Task Add(TEntity entity);
	}
}
