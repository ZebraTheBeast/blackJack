namespace BlackJack.BusinessLogic.Helpers
{
	public static class StringHelper
	{

        public static string PlayerDrawCard(long cardId)
		{
			return $"Player draw card with id {cardId}";
		}

		public static string NotEnoughPoints(int betValue)
		{
			return $"You doesn't have {betValue} points to bet";
		}

		public static string PlayerPlaceBet(int betValue)
		{
			return $"Player bet {betValue} points";
		}

		public static string PlayerValue(int playerValue, int dealerValue)
		{
			return $"Player has value {playerValue} against {dealerValue}";
		}

		public static string PlayerWin(int betValue)
		{
			return $"Player win {betValue} points";
		}

		public static string PlayerLose(int betValue)
		{
			return $"Player lose {betValue} points";
		}

		static public string OptionSetBet(string message)
		{
			message += "Please, place a bet.";
			return message;
		}
	}
}
