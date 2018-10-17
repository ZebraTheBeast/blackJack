using System.Threading.Tasks;
using BlackJack.ViewModels;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IGameService
    {
        Task<GetGameGameView> GetGame(long gameId);
        Task<ResponseBetGameView> PlaceBet(RequestBetGameView requestBetGameViewModel);
        Task<DrawGameView> DrawCard(long humanId);
        Task<StandGameView> Stand(long humanId);
    }
}
