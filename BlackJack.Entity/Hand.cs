using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	[Table("Hand")]
	public class Hand : BaseEntity
	{
		public int CardId { get; set; }
		public int PlayerInGameId { get; set; }

		[Write(false)]
		public virtual Card Card { get; set; }

	}
}
