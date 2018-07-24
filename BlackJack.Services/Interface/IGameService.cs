using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
namespace BlackJack.BLL.Interface
{
    public interface IGameService
    {
        GameViewModel GetGameViewModel();
        void StartGame(string humanName);
        bool BotTurn(int botId);
        void HumanDrawCard(int humanId);
        void RefreshGame();
        void UpdateScore(int playerId, int playerCardsValue, int dealerCardsValue);
        void EndTurn();
        void Dealing();
    }
}
