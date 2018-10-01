using System.Threading.Tasks;
using BlackJack.ViewModels;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IGameService
    {
        Task<GetGameViewModel> GetGame(long gameId);
        Task<ResponseBetGameViewModel> PlaceBet(RequestBetGameViewModel requestBetGameViewModel);
        Task<DrawGameViewModel> DrawCard(long humanId);
        Task<StandGameViewModel> Stand(long humanId);
    }
}
