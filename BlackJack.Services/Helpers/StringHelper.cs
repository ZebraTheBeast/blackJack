namespace BlackJack.BusinessLogic.Helpers
{
	public static class StringHelper
	{

		static public string OptionDrawCard = "Draw card or stand.";

		static public string OptionWin = "You win! ";

		static public string OptionLose = "You lose! ";

		static public string OptionDraw = "You have draw! ";

		public static string PlayerDrawCard(long cardId)
		{
			return $"Player draw card with id {cardId}";
		}

		public static string PlayerNotInGame()
		{
			return "Player not in game";
		}

		public static string AlreadyBet()
		{
			return "You already placed a bet";
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

		public static string PlayerDraw()
		{
			return $"Player has draw with Dealer";
		}

		public static string EmptyName()
		{
			return "Name field is empty! Please, input your name in it";
		}

		public static string EmptyLog()
		{
			return "Logs are empty";
		}

		public static string PlayerWin(int betValue)
		{
			return $"Player win {betValue} points";
		}

		public static string PlayerLose(int betValue)
		{
			return $"Player lose {betValue} points";
		}

		public static string BotJoinGame()
		{
			return "Bot join to the game";
		}

		public static string HumanJoinGame()
		{
			return $"Human join to the game";
		}

		public static string BotsNotInGame()
		{
			return "There are no bots in the game";
		}

		public static string NoLastGame()
		{
			return "You don't have last game, start a new one!";
		}

		public static string DealerNotInGame()
		{
			return "There is no dealer in the game";
		}

		public static string NoBetValue()
		{
			return "Bet is not allowed.";
		}

		public static string MaxBotsAmount()
		{
			return "Tooooo much bots";
		}

		public static string MinBotsAmount()
		{
			return "Tooooo few bots";
		}

		public static string PlayerContinueGame()
		{
			return "Player continue game";
		}

		static public string OptionSetBet(string message)
		{
			message += "Please, place a bet.";
			return message;
		}


	}
}
