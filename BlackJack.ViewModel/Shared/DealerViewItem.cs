using Newtonsoft.Json;

namespace BlackJack.ViewModels
{
	public class DealerViewItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("hand")]
		public HandViewItem Hand { get; set; }
	}
}
