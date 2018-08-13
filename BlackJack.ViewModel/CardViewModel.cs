using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity.Enum;
using Newtonsoft.Json;

namespace BlackJack.ViewModel
{
    public class CardViewModel
    {
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("value")]
		public int Value { get; set; }
		[JsonProperty("color")]
		public string Color { get; set; }
    }
}
