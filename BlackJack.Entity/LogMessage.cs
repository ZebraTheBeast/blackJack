using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Entities
{
	public class LogMessage
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public DateTime Logged { get; set; }
	}
}
