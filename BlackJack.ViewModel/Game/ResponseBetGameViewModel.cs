using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlackJack.ViewModels
{
	public class ResponseBetGameViewModel
	{
		[JsonProperty("dealer")]
		public DealerResponseBetGameViewModelItem Dealer { get; set; }
		[JsonProperty("human")]
		public PlayerResponseBetGameViewModelItem Human { get; set; }
		[JsonProperty("bots")]
		public List<PlayerResponseBetGameViewModelItem> Bots { get; set; }
		[JsonProperty("deck")]
		public List<int> Deck { get; set; }
		[JsonProperty("options")]
		public string Options { get; set; }
	}

	public class DealerResponseBetGameViewModelItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("hand")]
		public HandResponseBetGameViewModelItem Hand { get; set; }
	}

	public class PlayerResponseBetGameViewModelItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("points")]
		public int Points { get; set; }
		[JsonProperty("hand")]
		public HandResponseBetGameViewModelItem Hand { get; set; }
		[JsonProperty("betValue")]
		public int BetValue { get; set; }
	}

	public class HandResponseBetGameViewModelItem
	{
		[JsonProperty("cardList")]
		public List<CardResponseBetGameViewModelItem> CardList { get; set; }
		[JsonProperty("cardListValue")]
		public int CardListValue { get; set; }
	}

	public class CardResponseBetGameViewModelItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("value")]
		public int Value { get; set; }
		[JsonProperty("suit")]
		public string Suit { get; set; }
	}
}
