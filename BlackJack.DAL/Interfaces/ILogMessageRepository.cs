using BlackJack.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DAL.Interfaces
{
	public interface ILogMessageRepository
	{
		Task<IEnumerable<LogMessage>> GetAll();
	}
}
