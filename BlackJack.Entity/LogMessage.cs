using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	[Table ("LogInfo")]
	public class LogMessage : BaseEntity
	{
		public string Message { get; set; }
		public int GameId { get; set; }
		public int PlayerId { get; set; }
	}
}
