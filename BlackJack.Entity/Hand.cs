using Dapper.Contrib.Extensions;

namespace BlackJack.Entities.Properties
{
	[Table("Hand")]
	public class Hand : BaseEntity
	{
		public int PlayerId { get; set; }
		[ExplicitKey]
		public int CardId { get; set; }
		[ExplicitKey]
		public int GameId { get; set; }

		[Write(false)]
		public virtual Player Player { get; set; }
		[Write(false)]
		public virtual Card Card { get; set; }
		[Write(false)]
		public virtual Game Game { get; set; }

		[Write(false)]
		public override int Id { get; set; }
	}
}
