using Dapper.Contrib.Extensions;
using System.Collections.Generic;

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
		public virtual Player Player { get; set; }

		[Write(false)]
		public virtual List<Hand> CardsInHand { get; set; }
	}
}
