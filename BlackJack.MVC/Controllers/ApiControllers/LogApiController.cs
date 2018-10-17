﻿using BlackJack.BusinessLogic.Interfaces;
using BlackJack.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BlackJack.MVC.Controllers.ApiControllers
{
	public class LogApiController : ApiController
	{
		private ILogService _logService;
		private Logger _logger;

		public LogApiController(ILogService logService)
		{
			_logService = logService;
			_logger = LogManager.GetCurrentClassLogger();
		}

		public async Task<IHttpActionResult> GetLogs()
		{
			try
			{
				List<GetLogsLogView> logMessageViewModel = (await _logService.GetMessages()).ToList();
				return Ok(logMessageViewModel);
			}
			catch (Exception exception)
			{
				_logger.Error(exception.Message);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
			}
		}
	}
}
