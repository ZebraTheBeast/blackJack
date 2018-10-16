using BlackJack.DataAccess.Interfaces;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories
{
	public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{

		private string _connectionString;

		public GenericRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public virtual async Task<TEntity> GetById(long id)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var entity = await db.GetAsync<TEntity>(id);
				return entity;
			}
		}

		public async Task<List<TEntity>> GetAll()
		{
			using (var db = new SqlConnection(_connectionString))
			{
				var entities = (await db.GetAllAsync<TEntity>()).ToList();
				return entities;
			}
		}

		public async Task DeleteAll()
		{
			using (var db = new SqlConnection(_connectionString))
			{
				await db.DeleteAllAsync<TEntity>();
			}
		}

		public async Task<long> Add(TEntity item)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				long id = await db.InsertAsync<TEntity>(item);
				return id;
			}
		}

        public async Task Add(List<TEntity> items)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                await db.InsertAsync(items);
            }
        }
    }
}
