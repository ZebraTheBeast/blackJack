using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.BLL.Interface
{
    public interface IScoreService
    {
        Task<string> UpdateScore(int playerId, int playerCardsValue, int dealerCardsValue);
    }
}
