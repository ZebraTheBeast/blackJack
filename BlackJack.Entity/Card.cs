using BlackJack.Entities.Enums;
using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	[Table("Card")]
	public class Card : BaseEntity
	{
		public string Title { get; set; }
		public CardSuit Suit { get; set; }
		public int Value { get; set; }
	}
}
