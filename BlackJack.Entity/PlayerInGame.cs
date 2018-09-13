using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	[Table("PlayerInGame")]
	public class PlayerInGame : BaseEntity
	{
		[ExplicitKey]
		public int PlayerId { get; set; }
		[ExplicitKey]
		public int GameId { get; set; }
		public int Bet { get; set; }

		[Write(false)]
		public override int Id { get; set; }
	}
}
