using System;

namespace BlackJack.ViewModels
{
	public class GetLogsLogView
	{
		public long Id { get; set; }
		public DateTime CreationDate { get; set; }
		public string Message { get; set; }
		public long PlayerId { get; set; }
		public long GameId { get; set; }
	}
}
