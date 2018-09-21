using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlackJack.ViewModels
{
	public class StandGameViewModel
	{
		[JsonProperty("dealer")]
		public DealerStandGameViewModelItem Dealer { get; set; }
		[JsonProperty("human")]
		public PlayerStandGameViewModelItem Human { get; set; }
		[JsonProperty("bots")]
		public List<PlayerStandGameViewModelItem> Bots { get; set; }
		[JsonProperty("deck")]
		public List<int> Deck { get; set; }
		[JsonProperty("options")]
		public string Options { get; set; }
	}

	
	public class DealerStandGameViewModelItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("hand")]
		public HandStandGameViewModelItem Hand { get; set; }
	}

	public class PlayerStandGameViewModelItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("points")]
		public int Points { get; set; }
		[JsonProperty("hand")]
		public HandStandGameViewModelItem Hand { get; set; }
		[JsonProperty("betValue")]
		public int BetValue { get; set; }
	}

	public class HandStandGameViewModelItem
	{
		[JsonProperty("cardList")]
		public List<CardStandGameViewModelItem> CardList { get; set; }
		[JsonProperty("cardListValue")]
		public int CardListValue { get; set; }
	}

	public class CardStandGameViewModelItem
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
