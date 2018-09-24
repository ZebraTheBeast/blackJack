using Newtonsoft.Json;

namespace BlackJack.ViewModels
{
	public class DealerViewModelItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("hand")]
		public HandViewModelItem Hand { get; set; }
	}
}
