using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlackJack.ViewModels
{
	public class DrawGameViewModel
	{
		[JsonProperty("dealer")]
		public DealerViewModelItem Dealer { get; set; }
		[JsonProperty("human")]
		public PlayerViewModelItem Human { get; set; }
		[JsonProperty("bots")]
		public List<PlayerViewModelItem> Bots { get; set; }
		[JsonProperty("deck")]
		public List<int> Deck { get; set; }
		[JsonProperty("options")]
		public string Options { get; set; }
	}
}
