using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlackJack.ViewModels
{
	public class GetGameViewModel
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
