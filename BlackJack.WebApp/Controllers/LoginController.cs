using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.BusinessLogic.Helpers;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.Configurations;
using BlackJack.ViewModels;
using NLog;

namespace BlackJack.WebApp.Controllers
{
    public class LoginController : ApiController
    {
        private ILoginService _loginService;

        private Logger _logger;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;

            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpPost]
        public async Task<IHttpActionResult> StartGame([FromBody]RequestStartGameLoginView loginViewModel)
        {
            try
            {
                if(loginViewModel.BotsAmount < Constant.MinBotsAmount)
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

                var gameId = await _loginService.StartGame(loginViewModel.PlayerName, loginViewModel.BotsAmount);
                var response = new { gameId };

                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> LoadGame(string playerName)
        {
            try
            { 
                if (String.IsNullOrEmpty(playerName))
                {
                    throw new Exception(StringHelper.EmptyName());
                }
                long gameId = await _loginService.LoadGame(playerName);
                var response = new { gameId };

                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }
    }
}
