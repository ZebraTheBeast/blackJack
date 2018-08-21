using BlackJack.BusinessLogic.Interfaces;
using BlackJack.ViewModels;
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

		public LogController(ILogService logService)
		{
			_logService = logService;
		}

		public async Task<List<LogMessageViewModel>> GetLogs()
		{
            try
            {
                var model = (await _logService.GetMessages()).ToList();
                return model;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
            }
        }
	}
}
