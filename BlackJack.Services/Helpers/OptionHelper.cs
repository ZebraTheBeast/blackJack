namespace BlackJack.BusinessLogic.Helpers
{
	static public class OptionHelper
	{
		static public string OptionSetBet(string message)
		{
			message += "Please, place a bet.";
			return message;
		}

		static public string OptionDrawCard = "Draw card or stand.";

		static public string OptionWin = "You win! ";

		static public string OptionLose = "You lose! ";

		static public string OptionDraw = "You have draw! ";
	}
}
