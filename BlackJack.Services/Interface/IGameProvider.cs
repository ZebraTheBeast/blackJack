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
        Task<GameViewModel> GetGameViewModel();
        Task<GameViewModel> PlaceBet(int betValue);
        Task StartGame(string playerName);
        Task<GameViewModel> Draw(List<int> deck);
        Task<GameViewModel> Stand(List<int> deck);
    }
}
