using BlackJack.BusinessLogic.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BlackJack.MVC.Controllers.ApiControllers
{
	public class LoginApiController : ApiController
    {
		private ILoginService _loginService;

		public LoginApiController(ILoginService loginService)
		{
			_loginService = loginService;
		}

		[HttpPost]
		public async Task<int> StartGame([FromBody]string playerName)
		{
			try
			{
				if (playerName == "")
				{
					throw new Exception();
				}

				return await _loginService.StartGame(playerName, 3);
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(
						Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
			}
		}

		[HttpPost]
		public async Task<int> LoadGame([FromBody]string playerName)
		{
			try
			{
				if (playerName == "")
				{
					throw new Exception();
				}

				return await _loginService.LoadGame(playerName);
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(
						Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
			}
		}
	}
}
