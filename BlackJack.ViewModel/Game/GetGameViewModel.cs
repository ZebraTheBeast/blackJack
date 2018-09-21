using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlackJack.ViewModels
{
	public class GetGameViewModel
	{
		[JsonProperty("dealer")]
		public DealerGetGameViewModelItem Dealer { get; set; }
		[JsonProperty("human")]
		public PlayerGetGameViewModelItem Human { get; set; }
		[JsonProperty("bots")]
		public List<PlayerGetGameViewModelItem> Bots { get; set; }
		[JsonProperty("deck")]
		public List<int> Deck { get; set; }
		[JsonProperty("options")]
		public string Options { get; set; }
	}

	public class DealerGetGameViewModelItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("hand")]
		public HandGetGameViewModelItem Hand { get; set; }
	}

	public class PlayerGetGameViewModelItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("points")]
		public int Points { get; set; }
		[JsonProperty("hand")]
		public HandGetGameViewModelItem Hand { get; set; }
		[JsonProperty("betValue")]
		public int BetValue { get; set; }
	}

	public class HandGetGameViewModelItem
	{
		[JsonProperty("cardList")]
		public List<CardGetGameViewModelItem> CardList { get; set; }
		[JsonProperty("cardListValue")]
		public int CardListValue { get; set; }
	}

	public class CardGetGameViewModelItem
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
