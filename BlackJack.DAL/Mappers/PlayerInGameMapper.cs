using BlackJack.Entities;
using System;
using System.Collections.Generic;

namespace BlackJack.DataAccess.Mappers
{
    public class PlayerInGameMapper
    {
        public Func<PlayerInGame, Player, Hand, Card, PlayerInGame> GetMap(Dictionary<long, PlayerInGame> playerInGameDictionary)
        {
            Func<PlayerInGame, Player, Hand, Card, PlayerInGame> map = (playerInGame, player, hand, card) =>
            {
                PlayerInGame playerInGameResult;
                hand.Card = card;
                if (!playerInGameDictionary.TryGetValue(playerInGame.Id, out playerInGameResult))
                {
                    playerInGameDictionary.Add(playerInGame.Id, playerInGameResult = playerInGame);
                }

                if (playerInGameResult.CardsInHand == null)
                {
                    playerInGameResult.CardsInHand = new List<Hand>();
                }

                playerInGameResult.CardsInHand.Add(hand);
                return playerInGameResult;
            };

            return map;
        }
    }
}
