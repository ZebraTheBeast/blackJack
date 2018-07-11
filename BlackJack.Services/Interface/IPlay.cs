using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.Services.Interface
{
    public interface IPlay
    {
        void TakeCard(Player player, List<Card> deck);
        int GetCardValue(Player player);
        List<Card> GetCardsInHand(Player player);
        void PutPoints(Player player, int value);
        void WinPoints(Player player);
        void LosePoints(Player player);
        

    }
}
