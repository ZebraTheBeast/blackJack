using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BlackJack.BLL.Providers;
using BlackJack.ViewModel;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Helper;

namespace BlackJack.API.Controllers
{
    public class ValuesController : ApiController
    {
        IGameProvider _gameProvider;

        public ValuesController(IGameProvider gameProvider)
        {
            _gameProvider = gameProvider;
        }

        public async Task<GameViewModel> GetGameViewModel()
        {
            try
            {
                var gameViewModel = new GameViewModel();

                gameViewModel = await _gameProvider.GetGameViewModel();

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
        public async Task StartGame([FromBody]string playerName)
        {
            try
            {
                await _gameProvider.StartGame(playerName);
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(
                        Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
            }
        }

        [HttpPost]
        public async Task<GameViewModel> Bet([FromBody]int betValue)
        {
            try
            {
                var gameViewModel = new GameViewModel();
                gameViewModel = await _gameProvider.PlaceBet(betValue);
                return gameViewModel;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(
                       Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));
            }
        }

        [HttpPost]
        public async Task<GameViewModel> Draw([FromBody]List<int> deck)
        {
            try
            {
                var gameViewModel = new GameViewModel();
                gameViewModel = await _gameProvider.Draw(deck);
                return gameViewModel;
            }
            catch (Exception exception)
            {
                throw new HttpResponseException(
                       Request.CreateErrorResponse(HttpStatusCode.NotImplemented, exception.Message));

            }
        }

        [HttpPost]
        public async Task<GameViewModel> Stand([FromBody]List<int> deck)
        {
            try
            {
                var gameViewModel = new GameViewModel();
                gameViewModel = await _gameProvider.Stand(deck);
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
