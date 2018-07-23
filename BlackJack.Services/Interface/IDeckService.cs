using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Entity;

namespace BlackJack.BLL.Interface
{
    public interface IDeckService
    {
        void SaveCardsToDb();
        void RefreshAndShuffleDeck();
        void GiveCardFromDeck(int playerId);
        List<CardViewModel> GetDeck();
    }
}
