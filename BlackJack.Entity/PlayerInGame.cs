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
		public int BetValue { get; set; }
		public bool IsHuman { get; set; }

		[Write(false)]
		public virtual Player Player { get; set; }
	}
}
