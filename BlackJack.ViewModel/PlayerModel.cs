using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class PlayerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Points { get; set; }
        public HandModel Hand { get; set; }

        public PlayerModel()
        {
            Hand = new HandModel();
        }
    }
}
