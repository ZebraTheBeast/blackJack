using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace BlackJack.Entities
{
	[Table("Game")]
	public class Game : BaseEntity
	{
		public int HumanId { get; set; }
		
		[Write(false)]
		public virtual List<PlayerInGame> PlayersInGame { get; set; }
	}
}
