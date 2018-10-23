using System.Threading.Tasks;
using BlackJack.ViewModels;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IGameService
    {
        Task<ResponseBetGameView> PlaceBet(RequestBetGameView requestBetGameViewModel);
        Task<DrawGameView> DrawCard(long humanId);
        Task<StandGameView> Stand(long humanId);
        Task<ResponseStartGameGameView> StartGame(string playerName, int botsAmount);
        Task<LoadGameGameView> LoadGame(string playerName);
    }
}
