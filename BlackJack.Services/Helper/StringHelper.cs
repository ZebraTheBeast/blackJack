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

        

        public static string PlayerDrawCard(int playerId, int cardId)
        {
            return $"Player with id {playerId} draw card with id {cardId}";
        }

        public static string PlayerNotInGame()
        {
            return "Player not in game";
        }

        public static string DeckShuffled()
        {
            return "Deck was refreshed and shuffled.";
        }

        public static string NotEnoughPoints(int playerId, int betValue)
        {
            return $"Player with id {playerId} doesn't have {betValue} points to bet";
        }

        public static string PlayerMakeBet(int playerId, int betValue)
        {
            return $"Player with id {playerId} bet {betValue} points";
        }

        public static string PlayerValue(int playerId, int playerValue, int dealerValue)
        {
            return $"Player with id {playerId} has value {playerValue} against {dealerValue}";
        }

        public static string PlayerDraw(int playerId)
        {
            return $"Player with id {playerId} has draw with Dealer";
        }

        public static string PlayerWin(int playerId, int betValue)
        {
            return $"Player with id {playerId} win {betValue} points";
        }

        public static string PlayerLose(int playerId, int betValue)
        {
            return $"Player with id {playerId} lose {betValue} points";
        }

        public static string BotJoinGame(int botId)
        {
            return $"Bot with id {botId} join to the game";
        }

        public static string HumanJoinGame(int humanId)
        {
            return $"Human with id {humanId} join to the game";
        }
    }
}
