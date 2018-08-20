using BlackJack.BLL.Interfaces;
using BlackJack.DAL.Interfaces;
using BlackJack.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.BLL.Services
{
	public class LogService : ILogService
	{
		ILogMessageRepository _logMessageRepository;

		public LogService(ILogMessageRepository logMessageRepository)
		{
			_logMessageRepository = logMessageRepository;
		}

		public async Task<IEnumerable<LogMessageViewModel>> GetMessages()
		{
			var messagesModel = new List<LogMessageViewModel>();
			var messages = (await _logMessageRepository.GetAll()).ToList();
			foreach(var message in messages)
			{
				var messageModel = new LogMessageViewModel
				{
					Id = message.Id,
					Message = message.Message,
					Time = message.Logged
				};

				messagesModel.Add(messageModel);
			}

			return messagesModel;
		}
	}
}
