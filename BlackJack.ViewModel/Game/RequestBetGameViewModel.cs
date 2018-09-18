using Newtonsoft.Json;

namespace BlackJack.ViewModels
{
	public class RequestBetGameViewModel
    {
		[JsonProperty("betValue")]
		public int BetValue { get; set; }
		[JsonProperty("gameId")]
		public int GameId { get; set; }
    }
}
