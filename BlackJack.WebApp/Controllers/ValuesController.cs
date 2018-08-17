using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.ViewModel;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Helper;

namespace BlackJack.WebApp.Controllers
{
    public class ValuesController : ApiController
    {
        IGameProvider _gameProvider;

        public ValuesController(IGameProvider gameProvider)
        {
            _gameProvider = gameProvider;
        }

        [HttpGet]
        public async Task<GameViewModel> GetGameViewModel(int Id)
        {
            try
            {
                var gameViewModel = new GameViewModel();
                gameViewModel = await _gameProvider.GetGameViewModel(Id);

                if (gameViewModel.Bots.Count() == 0)
                {
                    throw new Exception(StringHelper.NoLastGame());
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
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
            }

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

                return await _gameProvider.StartGame(playerName);
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

                return await _gameProvider.LoadGame(playerName);
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
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
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
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
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
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
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
            }
        }
    }
}
