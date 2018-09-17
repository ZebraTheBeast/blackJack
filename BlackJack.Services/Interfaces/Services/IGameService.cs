using System.Threading.Tasks;
using BlackJack.ViewModels;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface IGameService
    {
        Task<GameViewModel> GetGameViewModel(int gameId);
        Task<GameViewModel> PlaceBet(int betValue, int humanId);
        Task<GameViewModel> Draw(int humanId);
        Task<GameViewModel> Stand(int humanId);
    }
}
