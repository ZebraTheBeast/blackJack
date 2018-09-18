using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.ViewModels;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.BusinessLogic.Helpers;

namespace BlackJack.MVC.Controllers
{
	public class GameApiController : ApiController
	{
		private IGameService _gameService;

		public GameApiController(IGameService gameService)
		{
			_gameService = gameService;
		}

		[HttpPost]
		public async Task<IHttpActionResult> GetGameViewModel([FromBody]int humanId)
		{
			try
			{
				var gameViewModel = new GetGameViewModel();

				gameViewModel = await _gameService.GetGame(humanId);

				if (gameViewModel.Bots.Count() == 0)
				{
					throw new Exception(StringHelper.BotsNotInGame());
				}

				if (gameViewModel.Dealer == null)
				{
					throw new Exception(StringHelper.DealerNotInGame());
				}

				if (gameViewModel.Human == null)
				{
					throw new Exception(StringHelper.PlayerNotInGame());
				}

				return Ok(gameViewModel);
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
			}

		}

		[HttpPost]
		public async Task<IHttpActionResult> Bet([FromBody]RequestBetGameViewModel requestBetGameViewModel)
		{
			try
			{
				var gameViewModel = new ResponseBetGameViewModel();
				gameViewModel = await _gameService.PlaceBet(requestBetGameViewModel);
				return Ok(gameViewModel);
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> Draw([FromBody]int humanId)
		{
			try
			{
				var gameViewModel = new DrawGameViewModel();
				gameViewModel = await _gameService.DrawCard(humanId);
				return Ok(gameViewModel);
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));

			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> Stand([FromBody]int humanId)
		{
			try
			{
				var standGameViewModel = new StandGameViewModel();
				standGameViewModel = await _gameService.Stand(humanId);
				return Ok(standGameViewModel);
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
			}
		}
	}
}
