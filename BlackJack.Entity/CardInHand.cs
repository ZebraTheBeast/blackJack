using Dapper.Contrib.Extensions;

namespace BlackJack.Entities
{
	[Table("CardInHand")]
	public class CardInHand : BaseEntity
	{
		public long CardId { get; set; }
		public long PlayerId { get; set; }
        public long GameId { get; set; }
	}
}
