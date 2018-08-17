using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlackJack.ViewModel
{
	public class HandViewModel
    {
		[JsonProperty("cardList")]
		public List<CardViewModel> CardList { get; set; }
		[JsonProperty("cardListValue")]
		public int CardListValue { get; set; }
		[JsonProperty("betValue")]
		public int BetValue { get; set; }
    }
}
