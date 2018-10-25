namespace BlackJack.BusinessLogic.Helpers
{
    public class UserMessages
    {
        static public string OptionDrawCard = "Draw card or stand.";

        static public string OptionWin = "You win! ";

        static public string OptionLose = "You lose! ";

        static public string OptionDraw = "You have draw! ";

        public static string PlayerNotInGame = "Player not in game";

        public static string AlreadyBet = "You already placed a bet";

        public static string PlayerDraw = "Player has draw with Dealer";

        public static string EmptyName = "Name field is empty! Please, input your name in it";

        public static string EmptyLog = "Logs are empty";

        public static string BotJoinGame = "Bot join to the game";

        public static string HumanJoinGame = "Human join to the game";

        public static string BotsNotInGame = "There are no bots in the game";

        public static string NoLastGame = "You don't have last game, start a new one!";

        public static string DealerNotInGame = "There is no dealer in the game";

        public static string NoBetValue = "Bet is not allowed.";

        public static string MaxBotsAmount = "Too much bots";

        public static string MinBotsAmount = "Too few bots";

        public static string PlayerContinueGame = "Player continue game";


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
