using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	public class BaseEntity
	{
		[Key]
		public int Id { get; set; }
	}
}
