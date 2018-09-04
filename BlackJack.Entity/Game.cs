using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	[Table("Game")]
	public class Game : BaseEntity
	{
		public int HumanId { get; set; }
	}
}
