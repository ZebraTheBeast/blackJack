using Dapper.Contrib.Extensions;

namespace BlackJack.Entities.Properties
{
	[Table("Hand")]
	public class Hand
	{
		[ExplicitKey]
		public int PlayerId { get; set; }
        public int CardId { get; set; }
		[ExplicitKey]
		public int GameId { get; set; }
    }
}
