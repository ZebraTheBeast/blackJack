using Newtonsoft.Json;

namespace BlackJack.ViewModels
{
	public class CardViewModel
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("value")]
		public int Value { get; set; }
		[JsonProperty("suit")]
		public string Suit { get; set; }
	}
}
