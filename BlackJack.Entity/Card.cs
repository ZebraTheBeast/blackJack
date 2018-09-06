using BlackJack.Entities.Enums;

namespace BlackJack.Entities
{
	public class Card : BaseEntity
    {
        public string Title { get; set; }
        public CardSuit Color { get; set; }
        public int Value { get; set; }
    }
}
