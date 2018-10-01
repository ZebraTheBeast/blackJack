using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	[Table("Hand")]
	public class Hand : BaseEntity
	{
		public long CardId { get; set; }
		public long PlayerInGameId { get; set; }

		[Write(false)]
		public virtual Card Card { get; set; }

	}
}
