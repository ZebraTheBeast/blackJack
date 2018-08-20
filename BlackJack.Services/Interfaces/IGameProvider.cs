using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.BLL.Interfaces
{
	public interface IGameProvider
    {
        Task<GameViewModel> GetGameViewModel(int gameId);
        Task<GameViewModel> PlaceBet(int betValue, int humanId);
        Task<int> StartGame(string playerName);
        Task<int> LoadGame(string playerName);
        Task<GameViewModel> Draw(int humanId);
        Task<GameViewModel> Stand(int humanId);
    }
}
