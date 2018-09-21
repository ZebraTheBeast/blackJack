using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlackJack.ViewModels
{
	public class DrawGameViewModel
	{
		[JsonProperty("dealer")]
		public DealerDrawGameViewModellItem Dealer { get; set; }
		[JsonProperty("human")]
		public PlayerDrawGameViewModelItem Human { get; set; }
		[JsonProperty("bots")]
		public List<PlayerDrawGameViewModelItem> Bots { get; set; }
		[JsonProperty("deck")]
		public List<int> Deck { get; set; }
		[JsonProperty("options")]
		public string Options { get; set; }
	}

	public class DealerDrawGameViewModellItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("hand")]
		public HandDrawGameViewModelItem Hand { get; set; }
	}

	public class PlayerDrawGameViewModelItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("points")]
		public int Points { get; set; }
		[JsonProperty("hand")]
		public HandDrawGameViewModelItem Hand { get; set; }
		[JsonProperty("betValue")]
		public int BetValue { get; set; }
	}

	public class HandDrawGameViewModelItem
	{
		[JsonProperty("cardList")]
		public List<CardDrawGameViewModelItem> CardList { get; set; }
		[JsonProperty("cardListValue")]
		public int CardListValue { get; set; }
	}

	public class CardDrawGameViewModelItem
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
