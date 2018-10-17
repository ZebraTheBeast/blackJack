using BlackJack.Entities;
using System;
using System.Collections.Generic;

namespace BlackJack.DataAccess.Mappers
{
    public class GameMapper
    {
        public Func<Game, PlayerInGame, Player, Game> GetMap(Dictionary<long, Game> gameDictionary)
        {
            Func<Game, PlayerInGame, Player, Game> map = (game, playerInGame, player) =>
            {
                Game gameResult;

                playerInGame.Player = player;

                if (!gameDictionary.TryGetValue(game.Id, out gameResult))
                {
                    gameDictionary.Add(game.Id, gameResult = game);
                }

                if (gameResult.PlayersInGame == null)
                {
                    gameResult.PlayersInGame = new List<PlayerInGame>();
                }
                gameResult.PlayersInGame.Add(playerInGame);

                return gameResult;
            };

            return map;
        }
    }
}
