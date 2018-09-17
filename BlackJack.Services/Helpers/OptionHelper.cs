namespace BlackJack.BusinessLogic.Helpers
{
	static public class OptionHelper
	{
		static public string OptionSetBet(string message)
		{
			message += "Please, place a bet.";
			return message;
		}

		static public string OptionDrawCard()
		{
			var message = "Draw card or stand.";
			return message;
		}

		static public string OptionWin()
		{
			var message = "You win! ";
			return message;
		}

		static public string OptionLose()
		{
			var message = "You lose! ";
			return message;
		}

		static public string OptionDraw()
		{
			var message = "You have draw! ";
			return message;
		}

		static public string OptionErrorBet()
		{
			var message = "You don't have enough points!";
			return message;
		}
	}
}
