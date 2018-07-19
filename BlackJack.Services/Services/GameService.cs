using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.DAL.Interface;
using BlackJack.BLL.Interface;
using BlackJack.Configuration.Constant;


namespace BlackJack.BLL.Services
{
    public class GameService : IGame
    {

        public GameModel Dealing(GameModel gameModel)
        {
            gameModel.Deck = DeckService.GetShuffledDeck();
            for(var i = 1; i < gameModel.Players.Count - 1; i++)
            {
                gameModel = PlaceBet(gameModel, gameModel.Players[i].Id, 100);
            }

            foreach (var player in gameModel.Players.ToArray())
            {
                gameModel = GiveCard(player.Id, gameModel);
                gameModel = GiveCard(player.Id, gameModel);
            }

            return gameModel;
        }

        public GameModel GiveCard(int playerId, GameModel gameModel)
        {
            var index = gameModel.Players.FindIndex(p => p.Id == playerId);

            gameModel.GameStat.Add($"Player {gameModel.Players[index].Name} draw card {gameModel.Deck[0].Title} of {gameModel.Deck[0].Color}.");

            gameModel.Players.Find(p => p.Id == playerId).Hand.CardList.Add(gameModel.Deck[0]);
            gameModel.Deck.Remove(gameModel.Deck[0]);
            gameModel.Players[index] = CountPlayerCardsValue(gameModel.Players[index]);

            

            return gameModel;
        }

        public GameModel PlayerTest(GameModel gameModel, PlayerModel player)
        {
            var players = new List<PlayerModel>
            {
                new PlayerModel { Id = 0, Name = "Dealer", Hand = new HandModel(){ CardList = new List<CardModel>() }, Points = 0 },
                new PlayerModel { Id = 1, Name = "Isaac Clarke", Hand = new HandModel(){ CardList = new List<CardModel>() }, Points = 1000 },
                new PlayerModel { Id = 2, Name = "Shredder", Hand = new HandModel(){ CardList = new List<CardModel>() }, Points = 1000 },
                new PlayerModel { Id = 3, Name = "Kun Lao", Hand = new HandModel(){ CardList = new List<CardModel>() }, Points = 1000 }
            };
            player.Hand = new HandModel();
            player.Hand.CardList = new List<CardModel>();
            gameModel.Players = players;
            gameModel.GameStat = new List<string>();
            gameModel.Players.Add(player);

            return gameModel;
        }

        private PlayerModel CountPlayerCardsValue(PlayerModel player)
        {
            player.Hand.CardListValue = 0;
            foreach (var card in player.Hand.CardList)
            {
                player.Hand.CardListValue += card.Value;
            }

            foreach (var card in player.Hand.CardList)
            {
                if ((card.Title == Constant.NameCardForBlackJack) && (player.Hand.CardListValue > Constant.WinValue))
                {
                    player.Hand.CardListValue -= Constant.ImageCardValue;
                }
            }
            return player;
        }

        public GameModel BotTurn(GameModel gameModel, PlayerModel player, int minValue)
        {
            do
            {
                if (!CombinationCheckerService.PlayerHandCardListValueLessThenPointsValue(gameModel.Players.First(p => p.Id == player.Id), minValue))
                {
                    return gameModel;
                }

                gameModel = GiveCard(player.Id, gameModel);

            } while (true);
        }

        public GameModel EditPoints(GameModel gameModel)
        {
            for (var i = 1; i < gameModel.Players.Count; i++)
            {
                gameModel = PointCheckerService.CheckPlayerWithDealer(gameModel.Players[i], gameModel.Players[0], gameModel);
            }

            return gameModel;
        }

        public GameModel EndTurn(GameModel gameModel)
        {
            for (var i = 0; i < gameModel.Players.Count; i++)
            {
                gameModel.Players[i].Hand.CardList = new List<CardModel>();
                gameModel.Players[i] = CountPlayerCardsValue(gameModel.Players[i]);
            }
            gameModel.Deck = new List<CardModel>();
            return gameModel;
        }

        public GameModel PlaceBet(GameModel gameModel, int playerId, int pointsValue)
        {
            // TODO - create StringService
            gameModel.GameStat.Add($"Player {gameModel.Players.Find(p => p.Id == playerId).Name} bet {pointsValue} points.");
            gameModel.Players.Find(p => p.Id == playerId).Hand.Points = pointsValue;
            return gameModel;
        }
    }
}
