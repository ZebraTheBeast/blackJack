using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlackJack.ViewModels
{
	public class HandViewModelItem
	{
		[JsonProperty("cardList")]
		public List<CardViewModelItem> CardList { get; set; }
		[JsonProperty("cardListValue")]
		public int CardListValue { get; set; }
	}
}
