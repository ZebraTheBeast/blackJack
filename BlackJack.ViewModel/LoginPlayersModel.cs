using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class LoginPlayersModel
    {
        public List<PlayerModel> PlayerList { get; set; }
        public PlayerModel Player { get; set; }

        public LoginPlayersModel()
        {
            PlayerList = new List<PlayerModel>();
        }
    }
}
