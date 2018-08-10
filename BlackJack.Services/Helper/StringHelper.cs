using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.BLL.Helper
{
    public static class StringHelper
    {
        public static string PlayerNotExist()
        {
            return "Player doesn't exist";
        }

        public static string NoPlayersInGame()
        {
            return "There are no players in the game";
        }

        public static string PlayerDrawCard(int playerId, int cardId, int gameId)
        {
            return $"Player with id {playerId} draw card with id {cardId} in game №{gameId}";
        }

        public static string PlayerNotInGame()
        {
            return "Player not in game";
        }

        public static string AlreadyBet()
        {
            return "You already placed a bet";
        }

        public static string DeckShuffled()
        {
            return "Deck was refreshed and shuffled";
        }

        public static string NotEnoughPoints(int playerId, int betValue)
        {
            return $"Player with id {playerId} doesn't have {betValue} points to bet";
        }

        public static string NotEnoughPoints(int betValue)
        {
            return $"You doesn't have {betValue} points to bet";
        }

        public static string PlayerPlaceBet(int playerId, int betValue, int gameId)
        {
            return $"Player with id {playerId} bet {betValue} points in game №{gameId}";
        }

        public static string PlayerValue(int playerId, int playerValue, int dealerValue, int gameId)
        {
            return $"Player with id {playerId} has value {playerValue} against {dealerValue} in game №{gameId}";
        }

        public static string PlayerDraw(int playerId, int gameId)
        {
            return $"Player with id {playerId} has draw with Dealer in game №{gameId}";
        }

		public static string EmptyName()
		{
			return "Name field is empty! Please, input your name in it :)";
		}

		public static string PlayerWin(int playerId, int betValue, int gameId)
        {
            return $"Player with id {playerId} win {betValue} points in game №{gameId}";
        }

        public static string PlayerLose(int playerId, int betValue, int gameId)
        {
            return $"Player with id {playerId} lose {betValue} points in game №{gameId}";
        }

        public static string BotJoinGame(int botId, int gameId)
        {
            return $"Bot with id {botId} join to the game №{gameId}";
        }

        public static string HumanJoinGame(int humanId, int gameId)
        {
            return $"Human with id {humanId} join to the game №{gameId}";
        }

        public static string BotsNotInGame()
        {
            return "There are no bots in the game";
        }

        public static string DealerNotInGame()
        {
            return "There is no dealer in the game";
        }

        public static string NoBetValue()
        {
            return "Player didn't bet";
        }

    }
}
