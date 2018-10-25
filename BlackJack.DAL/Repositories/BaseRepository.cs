using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories
{
	public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
	{

		private string _connectionString;

		public BaseRepository(string connectionString)
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

		public async Task<long> Add(TEntity entity)
		{
			using (var db = new SqlConnection(_connectionString))
			{
				long id = await db.InsertAsync<TEntity>(entity);
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

        public async Task<List<TEntity>> GetByIds(List<long> ids)
        {
            var sqlQuery  =  $"SELECT * FROM {typeof(TEntity).Name} WHERE Id in @ids";
            
            using (var db = new SqlConnection(_connectionString))
            {
                var items = (await db.QueryAsync<TEntity>(sqlQuery, new { ids })).ToList();

                return items;
            }
        }

        public async Task Delete(TEntity item)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                await db.DeleteAsync(item);
            }
        }
    }
}
