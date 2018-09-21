using BlackJack.BusinessLogic.Interfaces;
using BlackJack.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BlackJack.WebApp.Controllers
{
    public class LogController : ApiController
    {
		private ILogService _logService;
        Logger _logger;

        public LogController(ILogService logService)
		{
			_logService = logService;

            _logger = LogManager.GetCurrentClassLogger();
        }

		public async Task<IHttpActionResult> GetLogs()
		{
            try
            {
                List<GetLogsLogViewModel> logMessageViewModel = (await _logService.GetMessages()).ToList();
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
