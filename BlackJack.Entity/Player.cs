using BlackJack.Entities.Enums;
using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	[Table("Player")]
	public class Player : BaseEntity
	{
        public string Name { get; set; }
        public int Points { get; set; }
		public PlayerType Type { get; set; }
	}
}
