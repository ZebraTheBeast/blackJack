using Newtonsoft.Json;

namespace BlackJack.ViewModels
{
    public class RequestStartGameGameView
    {
        [JsonProperty("playerName")]
        public string PlayerName { get; set; }
        [JsonProperty("botsAmount")]
        public int BotsAmount { get; set; }
    }
}

