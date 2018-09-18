using System.Threading.Tasks;
using BlackJack.ViewModels;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IGameService
    {
        Task<GetGameViewModel> GetGame(int gameId);
        Task<ResponseBetGameViewModel> PlaceBet(RequestBetGameViewModel requestBetGameViewModel);
        Task<DrawGameViewModel> DrawCard(int humanId);
        Task<StandGameViewModel> Stand(int humanId);
    }
}
