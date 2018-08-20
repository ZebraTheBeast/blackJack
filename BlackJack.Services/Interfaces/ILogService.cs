using BlackJack.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.BLL.Interfaces
{
	public interface ILogService
	{
		Task<IEnumerable<LogMessageViewModel>> GetMessages();
	}
}
