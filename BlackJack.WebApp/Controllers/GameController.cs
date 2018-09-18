using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.ViewModels;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.BusinessLogic.Helpers;

namespace BlackJack.WebApp.Controllers
{
    public class GameController : ApiController
    {
        IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetGame([FromBody]int gameId)
        {
            try
            {
                var getGameViewModel = new GetGameViewModel();
                getGameViewModel = await _gameService.GetGame(gameId);

                if (getGameViewModel.Dealer == null)
                {
                    throw new Exception(StringHelper.DealerNotInGame());
                }

                if (getGameViewModel.Human == null)
                {
                    throw new Exception(StringHelper.PlayerNotInGame());
                }

                return Ok(getGameViewModel);
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> Bet([FromBody]RequestBetGameViewModel betViewModel)
        {
            try
            {
                var responseBetGameViewModel = new ResponseBetGameViewModel();
                responseBetGameViewModel = await _gameService.PlaceBet(betViewModel);
                return Ok(responseBetGameViewModel);
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Draw([FromBody]int gameId)
        {
            try
            {
                var drawGameViewModel = new DrawGameViewModel();
                drawGameViewModel = await _gameService.DrawCard(gameId);
                return Ok(drawGameViewModel);
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Stand([FromBody]int gameId)
        {
            try
            {
                var standGameViewModel = new StandGameViewModel();
                standGameViewModel = await _gameService.Stand(gameId);
                return Ok(standGameViewModel);
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }
    }
}
