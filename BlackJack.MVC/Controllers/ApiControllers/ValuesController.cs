using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.ViewModel;
using BlackJack.BLL.Interfaces;
using BlackJack.BLL.Helper;
using BlackJack.BLL.Services;
using System.Collections.Generic;

namespace BlackJack.MVC.Controllers
{
	public class ValuesController : ApiController
	{
		private IGameProvider _gameProvider;
		private ILogService _logService;

		public ValuesController(IGameProvider gameProvider, ILogService logService)
		{
			_gameProvider = gameProvider;
			_logService = logService;
		}

		public async Task<List<LogMessageViewModel>> GetLogs() 
		{
			var model = (await _logService.GetMessages()).ToList();
			return model;
		}

		[HttpPost]
		public async Task<GameViewModel> GetGameViewModel([FromBody]int humanId)
		{
			try
			{
				var gameViewModel = new GameViewModel();

				gameViewModel = await _gameProvider.GetGameViewModel(humanId);

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

				return gameViewModel;
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(
						Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
			}

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

				return await _gameProvider.StartGame(playerName);
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

				return await _gameProvider.LoadGame(playerName);
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(
						Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
			}
		}

		[HttpPost]
		public async Task<GameViewModel> Bet([FromBody]BetViewModel betViewModel)
		{
			try
			{
				var gameViewModel = new GameViewModel();
				gameViewModel = await _gameProvider.PlaceBet(betViewModel.BetValue, betViewModel.HumanId);
				return gameViewModel;
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(
					   Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
			}
		}

		[HttpPost]
		public async Task<GameViewModel> Draw([FromBody]int humanId)
		{
			try
			{
				var gameViewModel = new GameViewModel();
				gameViewModel = await _gameProvider.Draw(humanId);
				return gameViewModel;
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(
					   Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));

			}
		}

		[HttpPost]
		public async Task<GameViewModel> Stand([FromBody]int humanId)
		{
			try
			{
				var gameViewModel = new GameViewModel();
				gameViewModel = await _gameProvider.Stand(humanId);
				return gameViewModel;
			}
			catch (Exception exception)
			{
				throw new HttpResponseException(
					   Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
			}
		}
	}
}
