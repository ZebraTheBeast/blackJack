using Newtonsoft.Json;

namespace BlackJack.ViewModels
{
	public class RequestBetGameView
    {
		[JsonProperty("betValue")]
		public int BetValue { get; set; }
		[JsonProperty("gameId")]
		public long GameId { get; set; }
    }
}
