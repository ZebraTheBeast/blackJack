using Newtonsoft.Json;

namespace BlackJack.ViewModels
{
	public class PlayerViewItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("points")]
		public int Points { get; set; }
		[JsonProperty("hand")]
		public HandViewItem Hand { get; set; }
		[JsonProperty("betValue")]
		public int BetValue { get; set; }
	}
}
