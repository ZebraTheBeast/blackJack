using BlackJack.BusinessLogic.Interfaces;
using System;
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

		public LogApiController(ILogService logService)
		{
			_logService = logService;
		}

		public async Task<IHttpActionResult> GetLogs()
		{
			try
			{
				var logMessageViewModel = (await _logService.GetMessages()).ToList();
				return Ok(logMessageViewModel);
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
			}
		}
	}
}
