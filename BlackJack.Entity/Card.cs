using BlackJack.Entities.Enums;

namespace BlackJack.Entities
{
	public class Card : BaseEntity
    {
        public string Title { get; set; }
        public CardColor Color { get; set; }
        public int Value { get; set; }
    }
}
