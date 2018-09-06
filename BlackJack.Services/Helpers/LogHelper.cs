using NLog;

namespace BlackJack.BusinessLogic.Helpers
{
	public static class LogHelper
	{
		public static LogEventInfo GetEvent(int playerId, int gameId, string message)
		{
			LogEventInfo theEvent = new LogEventInfo(LogLevel.Info, "", message);
			theEvent.Properties["playerId"] = playerId;
			theEvent.Properties["gameId"] = gameId;
			return theEvent;
		}
	}
}
