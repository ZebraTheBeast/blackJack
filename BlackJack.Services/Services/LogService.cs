using BlackJack.BusinessLogic.Helpers;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.DataAccess.Interfaces;
using BlackJack.Entities;
using BlackJack.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Services
{
	public class LogService : ILogService
	{
		private ILogMessageRepository _logMessageRepository;

		public LogService(ILogMessageRepository logMessageRepository)
		{
			_logMessageRepository = logMessageRepository;
		}

		public async Task<IEnumerable<GetLogsLogView>> GetMessages()
		{
			var messagesModel = new List<GetLogsLogView>();
			List<LogMessage> messages = (await _logMessageRepository.GetAll()).ToList();

			if (messages.Count() == 0)
			{
				throw new Exception(UserMessages.EmptyLog);
			}

			foreach (var message in messages)
			{
				var messageModel = new GetLogsLogView
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
	}
}
