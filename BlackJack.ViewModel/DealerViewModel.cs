using Newtonsoft.Json;

namespace BlackJack.ViewModel
{
	public class DealerViewModel
    {
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("hand")]
		public HandViewModel Hand { get; set; }
    }
}
