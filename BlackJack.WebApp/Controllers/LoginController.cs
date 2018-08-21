using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.BusinessLogic.Helper;
using BlackJack.BusinessLogic.Interfaces;

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
        public async Task<int> StartGame([FromBody]string playerName)
        {
            try
            {
                if (String.IsNullOrEmpty(playerName))
                {
                    throw new Exception(StringHelper.EmptyName());
                }

                return await _loginService.StartGame(playerName);
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
