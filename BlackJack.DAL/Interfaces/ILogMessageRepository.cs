using BlackJack.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface ILogMessageRepository
	{
		Task<IEnumerable<LogMessage>> GetAll();
	}
}
