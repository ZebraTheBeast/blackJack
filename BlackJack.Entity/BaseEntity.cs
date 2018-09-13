using Dapper.Contrib.Extensions;
using System;

namespace BlackJack.Entities
{
	public class BaseEntity
	{
		[Key]
		public virtual int Id { get; set; }
		public DateTime CreationDate { get; set; }
		
		public BaseEntity()
		{
			CreationDate = DateTime.Now;
		}
	}
}
