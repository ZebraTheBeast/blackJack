using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlackJack.ViewModels
{
	public class HandViewModelItem
	{
		[JsonProperty("cardsInHand")]
		public List<CardViewModelItem> CardsInHand { get; set; }
		[JsonProperty("cardsInHandValue")]
		public int CardsInHandValue { get; set; }
	}
}
