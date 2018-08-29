using BlackJack.BusinessLogic.Helper;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.DataAccess.Interfaces;
using BlackJack.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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

			var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\"));
			NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(path + "BlackJack.Configuration\\Nlog.config", true);
		}

		public async Task<IEnumerable<LogMessageViewModel>> GetMessages()
		{
			var logger = NLog.LogManager.GetCurrentClassLogger();
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
				logger.Error(exception.Message);
				throw exception;
			}
		}
	}
}
