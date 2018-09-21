using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace BlackJack.Entities
{
	[Table("Game")]
	public class Game : BaseEntity
	{	
		[Write(false)]
		public virtual List<PlayerInGame> PlayersInGame { get; set; }
	}
}
