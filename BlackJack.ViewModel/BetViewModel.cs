using Newtonsoft.Json;

namespace BlackJack.ViewModels
{
	public class BetViewModel
    {
		[JsonProperty("betValue")]
		public int BetValue { get; set; }
		[JsonProperty("humanId")]
		public int HumanId { get; set; }
    }
}
