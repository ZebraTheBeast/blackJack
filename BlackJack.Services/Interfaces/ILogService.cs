using BlackJack.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface ILogService
	{
		Task<IEnumerable<LogMessageViewModel>> GetMessages();
	}
}
