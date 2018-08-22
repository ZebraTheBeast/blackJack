using System;

namespace BlackJack.Entities
{
	public class LogMessage : BaseEntity
	{
		public string Message { get; set; }
		public DateTime Logged { get; set; }
	}
}
