using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.BusinessLogic.Helper;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.ViewModels;
using BlackJack.Configurations;

namespace BlackJack.WebApp.Controllers
{
    public class LoginController : ApiController
    {
        private ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<int> StartGame([FromBody]LoginViewModel loginViewModel)
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

                return await _loginService.StartGame(loginViewModel.PlayerName, loginViewModel.BotsAmount);
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
            }
        }

        [HttpGet]
        public async Task<int> LoadGame(string playerName)
        {
            try
            {
                if (String.IsNullOrEmpty(playerName))
                {
                    throw new Exception(StringHelper.EmptyName());
                }

                return await _loginService.LoadGame(playerName);
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
            }
        }
    }
}
