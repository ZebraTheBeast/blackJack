using BlackJack.BusinessLogic.Helpers;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.DataAccess.Interfaces;
using BlackJack.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Services
{
	public class LogService : ILogService
	{
		ILogMessageRepository _logMessageRepository;

		Logger _logger;

		public LogService(ILogMessageRepository logMessageRepository)
		{
			var path = string.Empty;

			_logMessageRepository = logMessageRepository;

			_logger = LogManager.GetCurrentClassLogger();
		}

		public async Task<IEnumerable<GetLogsLogViewModel>> GetMessages()
		{
			
			try
			{
				var messagesModel = new List<GetLogsLogViewModel>();
				var messages = (await _logMessageRepository.GetAll()).ToList();

				if (messages.Count() == 0)
				{
					throw new Exception(StringHelper.EmptyLog());
				}

				foreach (var message in messages)
				{
					var messageModel = new GetLogsLogViewModel
					{
						Id = message.Id,
						Message = message.Message,
						CreationDate = message.CreationDate,
						GameId = message.GameId,
						PlayerId = message.PlayerId
					};

					messagesModel.Add(messageModel);
				}

				return messagesModel;
			}
			catch (Exception exception)
			{
				_logger.Error(exception.Message);
				throw exception;
			}
		}
	}
}
