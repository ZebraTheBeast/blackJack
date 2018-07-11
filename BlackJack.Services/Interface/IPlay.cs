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
        void TakeCard(PlayerModel player, List<Card> deck);
        int GetCardValue(PlayerModel player);
        List<Card> GetCardsInHand(PlayerModel player);
        void PutPoints(PlayerModel player, int value);
        void WinPoints(PlayerModel player);
        void LosePoints(PlayerModel player);
        

    }
}
