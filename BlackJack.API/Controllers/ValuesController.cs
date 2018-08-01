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

            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.GetGameViewModel();

            if (gameViewModel.Bots.Count() == 0)
            {
                throw new Exception("ass");
            }

            if (gameViewModel.Dealer == null)
            {
                throw new Exception("ass");
            }
            gameViewModel.Human = null;
            if (gameViewModel.Human == null)
            {

                // zalupa
                var message = "No human in game";
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotImplemented, message));
            }
            return gameViewModel;

        }

        [HttpPost]
        public async Task StartGame([FromBody]string playerName)
        {
            await _gameProvider.StartGame(playerName);
        }

        [HttpPost]
        public async Task<GameViewModel> Bet([FromBody]BetViewModel betViewModel)
        {
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.PlaceBet(betViewModel.HumanId, betViewModel.BetValue);
            return gameViewModel;
        }

        [HttpPost]
        public async Task<GameViewModel> Draw([FromBody]DrawViewModel drawViewModel)
        {
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.Draw(drawViewModel.HumanId, drawViewModel.Deck);
            return gameViewModel;
        }

        [HttpPost]
        public async Task<GameViewModel> Stand([FromBody]List<int> deck)
        {
            var gameViewModel = new GameViewModel();
            gameViewModel = await _gameProvider.BotTurn(deck);
            return gameViewModel;
        }
    }
}
