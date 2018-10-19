using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.ViewModels;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.BusinessLogic.Helpers;
using NLog;

namespace BlackJack.WebApp.Controllers
{
    public class GameController : ApiController
    {
        IGameService _gameService;

        private Logger _logger;
        
        public GameController(IGameService gameService)
        {
            _gameService = gameService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetGame([FromBody]long gameId)
        {
            try
            {
                var getGameViewModel = new GetGameGameView();
                getGameViewModel = await _gameService.GetGame(gameId);

                if (getGameViewModel.Dealer == null)
                {
                    throw new Exception(UserMessages.DealerNotInGame);
                }

                if (getGameViewModel.Human == null)
                {
                    throw new Exception(UserMessages.PlayerNotInGame);
                }

                return Ok(getGameViewModel);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> Bet([FromBody]RequestBetGameView betViewModel)
        {
            try
            {
                var responseBetGameViewModel = new ResponseBetGameView();
                responseBetGameViewModel = await _gameService.PlaceBet(betViewModel);
                return Ok(responseBetGameViewModel);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Draw([FromBody]long gameId)
        {
            try
            {
                var drawGameViewModel = new DrawGameView();
                drawGameViewModel = await _gameService.DrawCard(gameId);
               
                return Ok(drawGameViewModel);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Stand([FromBody]long gameId)
        {
            try
            {
                var standGameViewModel = new StandGameView();
                standGameViewModel = await _gameService.Stand(gameId);
                return Ok(standGameViewModel);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }
    }
}
