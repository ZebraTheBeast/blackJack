using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
