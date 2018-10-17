﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlackJack.ViewModels
{
	public class ResponseBetGameView
	{
		[JsonProperty("dealer")]
		public DealerViewItem Dealer { get; set; }
		[JsonProperty("human")]
		public PlayerViewItem Human { get; set; }
		[JsonProperty("bots")]
		public List<PlayerViewItem> Bots { get; set; }
		[JsonProperty("deck")]
		public List<long> Deck { get; set; }
		[JsonProperty("options")]
		public string Options { get; set; }
	}
}
