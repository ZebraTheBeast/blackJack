using BlackJack.BusinessLogic.Helper;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.Configurations;
using BlackJack.ViewModels;
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
		public async Task<IHttpActionResult> StartGame([FromBody]LoginViewModel loginViewModel)
		{
			try
			{
				if (loginViewModel.BotsAmount < Constant.MinBotsAmount)
				{
					throw new Exception(StringHelper.MinBotsAmount());
				}

				if (loginViewModel.BotsAmount > Constant.MaxBotsAmount)
				{
					throw new Exception(StringHelper.MaxBotsAmount());
				}

				if (String.IsNullOrEmpty(loginViewModel.PlayerName))
				{
					throw new Exception(StringHelper.EmptyName());
				}

				var humanId = await _loginService.StartGame(loginViewModel.PlayerName, loginViewModel.BotsAmount);
				return Ok(humanId);
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> LoadGame([FromBody]string playerName)
		{
			try
			{
				if (String.IsNullOrEmpty(playerName))
				{
					throw new Exception(StringHelper.EmptyName());
				}
				var humanId = await _loginService.LoadGame(playerName);
				return Ok(humanId);
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
			}
		}
	}
}
