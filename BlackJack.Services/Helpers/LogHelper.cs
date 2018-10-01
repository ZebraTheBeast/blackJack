using NLog;

namespace BlackJack.BusinessLogic.Helpers
{
	public static class LogHelper
	{
		public static LogEventInfo GetEvent(long playerId, long gameId, string message)
		{
			LogEventInfo theEvent = new LogEventInfo(LogLevel.Info, string.Empty, message);
			theEvent.Properties["playerId"] = playerId;
			theEvent.Properties["gameId"] = gameId;
			return theEvent;
		}
	}
}
