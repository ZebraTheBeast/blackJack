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
        Task<GameViewModel> PlaceBet(int humanId, int betValue);
        Task StartGame(string playerName);
        Task<GameViewModel> Draw(int humanId, List<int> deck);
        Task<GameViewModel> BotTurn(List<int> deck);
    }
}
