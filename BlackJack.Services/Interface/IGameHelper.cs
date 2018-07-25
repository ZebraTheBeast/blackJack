using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
namespace BlackJack.BLL.Interface
{
    public interface IGameHelper
    {
        Task<GameViewModel> BotTurn(List<int> deck);
        Task<GameViewModel> PlaceBet(int humanId, int pointsValue);
        Task<GameViewModel> Draw(int humanId, List<int> deck);
        Task<GameViewModel> StartGame(string playerName);
    }
}
