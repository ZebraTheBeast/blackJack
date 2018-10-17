using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlackJack.ViewModels
{
	public class HandViewItem
	{
		[JsonProperty("cardsInHand")]
		public List<CardViewItem> CardsInHand { get; set; }
		[JsonProperty("cardsInHandValue")]
		public int CardsInHandValue { get; set; }
	}
}
