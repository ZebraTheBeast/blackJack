using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.Configuration.Constant
{
    public class Constant
    {
        public const int WinValue = 21;
        public const int AceCardValue = 11;
        public const int ImageCardValue = 10;
        public const int DeckSize = 52;
        public const int NumberStartCard = 2;
        public const int CountNumberCard = 9;
        public const int NumberCardForBlackJack = 2;
        public const string NameCardForBlackJack = "Ace";
        public static List<PlayerModel> StartPlayers = new List<PlayerModel>
            {
                new PlayerModel { Id = 0, Name = "Dealer", Hand = new HandModel(){ CardList = new List<CardModel>() }, Points = 0 },
                new PlayerModel { Id = 1, Name = "Isaac Clarke", Hand = new HandModel(){ CardList = new List<CardModel>() }, Points = 1000 },
                new PlayerModel { Id = 2, Name = "Shredder", Hand = new HandModel(){ CardList = new List<CardModel>() }, Points = 1000 },
                new PlayerModel { Id = 3, Name = "Kun Lao", Hand = new HandModel(){ CardList = new List<CardModel>() }, Points = 1000 }
            };
    }
}
