using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	[Table("Game")]
	public class Game : BaseEntity
	{
		public int HumanId { get; set; }

		[Write(false)]
		public virtual Player Human { get; set; }
	}
}
