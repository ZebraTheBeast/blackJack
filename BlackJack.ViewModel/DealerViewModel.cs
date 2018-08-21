using Newtonsoft.Json;

namespace BlackJack.ViewModels
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
