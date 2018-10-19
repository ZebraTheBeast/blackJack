using BlackJack.Entities;
using System;
using System.Collections.Generic;

namespace BlackJack.DataAccess.Mappers
{
    public class PlayerInGameMapper
    {
        public Func<PlayerInGame, Player, CardInHand, Card, PlayerInGame> GetMap(Dictionary<long, PlayerInGame> playerInGameDictionary)
        {
            Func<PlayerInGame, Player, CardInHand, Card, PlayerInGame> map = (playerInGame, player, hand, card) =>
            {
                PlayerInGame playerInGameResult;

                if (!playerInGameDictionary.TryGetValue(playerInGame.Id, out playerInGameResult))
                {
                    playerInGameDictionary.Add(playerInGame.Id, playerInGameResult = playerInGame);
                }

                if (playerInGameResult.CardsInHand == null)
                {
                    playerInGameResult.CardsInHand = new List<Card>();
                }

                playerInGameResult.CardsInHand.Add(card);

                return playerInGameResult;
            };

            return map;
        }
    }
}
