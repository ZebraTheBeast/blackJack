using BlackJack.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface ILogService
	{
		Task<IEnumerable<LogMessageViewModel>> GetMessages();
	}
}
