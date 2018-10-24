using BlackJack.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface IBaseRepository<TEntity> where TEntity : BaseEntity
	{
		Task<TEntity> GetById(long id);
        Task<List<TEntity>> GetByIds(List<long> ids);
		Task<List<TEntity>> GetAll();
		Task DeleteAll();
		Task<long> Add(TEntity item);
        Task Add(List<TEntity> items);
	}
}
