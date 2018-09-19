using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	[Table ("LogInfo")]
	public class LogMessage : BaseEntity
	{
		public string Message { get; set; }
		public int GameId { get; set; }
		public int PlayerId { get; set; }

		[Write(false)]
		public virtual Game Game { get; set; }
		[Write(false)]
		public virtual Player Player { get; set; }
	}
}
