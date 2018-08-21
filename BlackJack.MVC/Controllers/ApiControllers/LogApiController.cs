using BlackJack.BusinessLogic.Interfaces;
using BlackJack.ViewModels;
using System.Collections.Generic;
using System.Linq;
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

		public async Task<List<LogMessageViewModel>> GetLogs()
		{
			var model = (await _logService.GetMessages()).ToList();
			return model;
		}
	}
}
