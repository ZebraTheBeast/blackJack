using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class PlayerViewModel
    {
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("points")]
		public int Points { get; set; }
		[JsonProperty("hand")]
		public HandViewModel Hand { get; set; }
    }
}
