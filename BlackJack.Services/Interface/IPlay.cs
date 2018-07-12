using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.BLL.Interface
{
    public interface IPlay
    {
        void TakeCard(PlayerModel player, List<CardModel> deck);
        int GetCardValue(PlayerModel player);
        List<CardModel> GetCardsInHand(PlayerModel player);
        void PutPoints(PlayerModel player, int value);
        void EmptyHand(PlayerModel player);
    }
}
