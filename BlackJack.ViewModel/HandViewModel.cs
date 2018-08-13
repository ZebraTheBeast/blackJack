using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
