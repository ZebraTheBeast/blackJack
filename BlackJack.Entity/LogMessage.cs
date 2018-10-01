using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	[Table ("LogInfo")]
	public class LogMessage : BaseEntity
	{
		public string Message { get; set; }
		public long GameId { get; set; }
		public long PlayerId { get; set; }
	}
}
