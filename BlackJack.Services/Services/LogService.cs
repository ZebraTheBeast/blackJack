using BlackJack.BusinessLogic.Helper;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.DataAccess.Interfaces;
using BlackJack.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Services
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
			try
			{
				var messagesModel = new List<LogMessageViewModel>();
				var messages = (await _logMessageRepository.GetAll()).ToList();

				if (messages.Count() == 0)
				{
					throw new Exception(StringHelper.EmptyLog());
				}

				foreach (var message in messages)
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
			catch (Exception exception)
			{
				throw exception;
			}
		}
	}
}
