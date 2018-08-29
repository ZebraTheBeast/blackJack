using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.ViewModels;
using BlackJack.BusinessLogic.Interfaces;
using BlackJack.BusinessLogic.Helper;

namespace BlackJack.WebApp.Controllers
{
    public class GameController : ApiController
    {
        IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<GameViewModel> GetGameViewModel(int Id)
        {
            try
            {
                var gameViewModel = new GameViewModel();
                gameViewModel = await _gameService.GetGameViewModel(Id);

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
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }

        }

       

        [HttpPost]
        public async Task<GameViewModel> Bet([FromBody]BetViewModel betViewModel)
        {
            try
            {
                var gameViewModel = new GameViewModel();
                gameViewModel = await _gameService.PlaceBet(betViewModel.BetValue, betViewModel.HumanId);
                return gameViewModel;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

        [HttpPost]
        public async Task<GameViewModel> Draw([FromBody]int humanId)
        {
            try
            {
                var gameViewModel = new GameViewModel();
                gameViewModel = await _gameService.Draw(humanId);
                return gameViewModel;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

        [HttpPost]
        public async Task<GameViewModel> Stand([FromBody]int humanId)
        {
            try
            {
                var gameViewModel = new GameViewModel();
                gameViewModel = await _gameService.Stand(humanId);
                return gameViewModel;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }
    }
}
