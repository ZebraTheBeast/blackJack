using System;

namespace BlackJack.Entities
{
	public class Player : BaseEntity
	{
        public string Name { get; set; }
        public int Points { get; set; }
		public DateTime CreationDate { get; }

		public Player()
		{
			CreationDate = DateTime.Now;
		}
    }
}
