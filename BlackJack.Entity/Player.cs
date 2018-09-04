using Dapper.Contrib.Extensions;
using System;

namespace BlackJack.Entities
{
	[Table("Player")]
	public class Player : BaseEntity
	{
        public string Name { get; set; }
        public int Points { get; set; }
		public DateTime CreationDate { get; set; }

		public Player()
		{
			CreationDate = DateTime.Now;
		}
    }
}
