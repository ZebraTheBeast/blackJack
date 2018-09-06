using System;

namespace BlackJack.ViewModels
{
	public class LogMessageViewModel
	{
		public int Id { get; set; }
		public DateTime Time { get; set; }
		public string Message { get; set; }
		public int PlayerId { get; set; }
		public int GameId { get; set; }
	}
}
