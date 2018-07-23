using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class GameModel
    {
        public List<PlayerModel> Players { get; set; }
        public List<CardModel> Deck { get; set; }
        public List<string> GameStats { get; set; }
        public int ButtonPushed { get; set; }
        public string Options { get; set; }
    }
}
