using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace BlackJack.Entities
{
	[Table("PlayerInGame")]
	public class PlayerInGame : BaseEntity
	{
		[ExplicitKey]
		public long PlayerId { get; set; }
		[ExplicitKey]
		public long GameId { get; set; }
		public int BetValue { get; set; }
		public bool IsHuman { get; set; }

		[Write(false)]
		public virtual Player Player { get; set; }
		[Write(false)]
		public virtual List<Card> CardsInHand { get; set; }
	}
}
