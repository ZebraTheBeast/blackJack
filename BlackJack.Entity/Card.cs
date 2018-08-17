using BlackJack.Entity.Enum;

namespace BlackJack.Entity
{
	public class Card
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public CardColor Color { get; set; }
        public int Value { get; set; }
    }
}
