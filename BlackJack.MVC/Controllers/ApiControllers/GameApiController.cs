using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.ViewModels;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.BusinessLogic.Helpers;
using NLog;
using BlackJack.Configurations;

namespace BlackJack.MVC.Controllers
{
	public class GameApiController : ApiController
	{
		IGameService _gameService;

		private Logger _logger;

		public GameApiController(IGameService gameService)
		{
			_gameService = gameService;
			_logger = LogManager.GetCurrentClassLogger();
		}

        [HttpPost]
        public async Task<IHttpActionResult> StartMatch([FromBody]RequestStartMatchGameView loginViewModel)
        {
            try
            {
                if (loginViewModel.BotsAmount < Constant.MinBotsAmount)
                {
                    throw new Exception(UserMessages.MinBotsAmount);
                }

                if (loginViewModel.BotsAmount > Constant.MaxBotsAmount)
                {
                    throw new Exception(UserMessages.MaxBotsAmount);
                }

                if (String.IsNullOrEmpty(loginViewModel.PlayerName))
                {
                    throw new Exception(UserMessages.EmptyName);
                }

                var startMatchGameView = await _gameService.StartGame(loginViewModel.PlayerName, loginViewModel.BotsAmount);

                return Ok(startMatchGameView);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> LoadMatch(string playerName)
        {
            try
            {
                if (String.IsNullOrEmpty(playerName))
                {
                    throw new Exception(UserMessages.EmptyName);
                }
                var loadMatchGameView = await _gameService.LoadGame(playerName);

                return Ok(loadMatchGameView);
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
