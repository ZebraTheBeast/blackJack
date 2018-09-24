using BlackJack.BusinessLogic.Helpers;
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
			var path = string.Empty;

			_logMessageRepository = logMessageRepository;
		}

		public async Task<IEnumerable<GetLogsLogViewModel>> GetMessages()
		{
			var messagesModel = new List<GetLogsLogViewModel>();
			var messages = (await _logMessageRepository.GetAllMessages()).ToList();

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
	}
}
