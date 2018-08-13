using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class GameViewModel
    {	
		[JsonProperty("dealer")]
        public DealerViewModel Dealer { get; set; }
		[JsonProperty("human")]
		public PlayerViewModel Human { get; set; }
		[JsonProperty("bots")]
		public List<PlayerViewModel> Bots { get; set; }
		[JsonProperty("deck")]
		public List<int> Deck { get; set; }
		[JsonProperty("options")]
		public string Options { get; set; }
    }
}
