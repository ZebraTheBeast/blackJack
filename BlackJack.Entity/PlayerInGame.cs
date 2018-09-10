using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	[Table("PlayerInGame")]
	public class PlayerInGame
	{
		[ExplicitKey]
		public int PlayerId { get; set; }
		[ExplicitKey]
		public int GameId { get; set; }
		public int Bet { get; set; }
	}
}
