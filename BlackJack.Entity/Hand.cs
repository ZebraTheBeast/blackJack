using Dapper.Contrib.Extensions;

namespace BlackJack.Entities.Properties
{
	[Table("Hand")]
	public class Hand : BaseEntity
	{
		[ExplicitKey]
		public int PlayerId { get; set; }
        public int CardId { get; set; }
		[ExplicitKey]
		public int GameId { get; set; }

		[Write(false)]
		public override int Id { get; set; }
	}
}
