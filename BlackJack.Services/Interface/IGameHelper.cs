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
        GameViewModel BotTurn(List<int> deck);
        GameViewModel PlaceBet(int humanId, int pointsValue);
        GameViewModel Draw(int humanId, List<int> deck);
        GameViewModel StartGame(string playerName);
    }
}
