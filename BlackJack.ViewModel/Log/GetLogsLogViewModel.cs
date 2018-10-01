using System;

namespace BlackJack.ViewModels
{
	public class GetLogsLogViewModel
	{
		public long Id { get; set; }
		public DateTime CreationDate { get; set; }
		public string Message { get; set; }
		public int PlayerId { get; set; }
		public int GameId { get; set; }
	}
}
