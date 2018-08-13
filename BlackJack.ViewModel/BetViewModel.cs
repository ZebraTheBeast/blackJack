using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class BetViewModel
    {
		[JsonProperty("betValue")]
		public int BetValue { get; set; }
		[JsonProperty("humanId")]
		public int HumanId { get; set; }
    }
}
