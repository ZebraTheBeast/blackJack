using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.BLL.Interface
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
