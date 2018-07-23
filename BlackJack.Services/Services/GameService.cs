using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.DAL.Interface;
using BlackJack.Configuration.Constant;
using BlackJack.DAL.Repository;
using AutoMapper;
using BlackJack.Entity;


namespace BlackJack.BLL.Services
{
    public static class GameService
    {
        static PlayerRepository _playerRepository = new PlayerRepository();

        public static GameModel Dealing(GameModel gameModel)
        {
            gameModel.Deck = DeckService.GetShuffledDeck();

            for (var i = 1; i < gameModel.Players.Count - 1; i++)
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

        public static GameModel GiveCard(int playerId, GameModel gameModel)
        {
            var index = gameModel.Players.FindIndex(p => p.Id == playerId);
            StringService.PlayerDrawCard(gameModel, index);
            gameModel.Players.Find(p => p.Id == playerId).Hand.CardList.Add(gameModel.Deck[0]);
            gameModel.Deck.Remove(gameModel.Deck[0]);
            gameModel.Players[index] = CountPlayerCardsValue(gameModel.Players[index]);

            return gameModel;
        }

        public static GameModel StartGame(string playerName)
        {
            var players = new List<PlayerModel>();
            var gameModel = new GameModel();

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Player, PlayerModel>());

            var playerEntity = _playerRepository.GetByName(playerName);
            var humanPlayer = Mapper.Map<Player, PlayerModel>(playerEntity);
            var playersEntity = _playerRepository.GetBots();

            foreach(var item in playersEntity)
            {
                players.Add(Mapper.Map<Player, PlayerModel>(item));
            }

            players.Add(humanPlayer);

            for(var i = 0; i < players.Count; i++)
            {
                players[i].Hand = new HandModel {CardList = new List<CardModel>() };
            }

            gameModel.Players = players;
            gameModel.GameStats = new List<string>();
            gameModel.Deck = new List<CardModel>();
            gameModel = OptionService.OptionSetBet(gameModel);

            return gameModel;
        }

        private static PlayerModel CountPlayerCardsValue(PlayerModel player)
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

            if (CombinationCheckerService.PlayerHandCardListIsBlackJack(player))
            {
                player.Hand.CardListValue = Constant.WinValue;
            }

            return player;
        }

        public static GameModel BotTurn(GameModel gameModel, PlayerModel player, int minValue)
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

        public static GameModel EditPoints(GameModel gameModel)
        {
            for (var i = 1; i < gameModel.Players.Count; i++)
            {
                gameModel = PointCheckerService.CheckPlayerWithDealer(gameModel.Players[i], gameModel.Players[0], gameModel);
                _playerRepository.Update(Mapper.Map<PlayerModel, Player>(gameModel.Players[i]));
            }

            gameModel = OptionService.OptionSetBet(gameModel);
            return gameModel;
        }

        public static GameModel EndTurn(GameModel gameModel)
        {
            for (var i = 0; i < gameModel.Players.Count; i++)
            {
                gameModel.Players[i].Hand.CardList = new List<CardModel>();
                gameModel.Players[i] = CountPlayerCardsValue(gameModel.Players[i]);
            }

            gameModel.Deck = new List<CardModel>();

            return gameModel;
        }

        public static GameModel PlaceBet(GameModel gameModel, int playerId, int pointsValue)
        {
            gameModel.Players.Find(p => p.Id == playerId).Hand.Points = pointsValue;
            gameModel = StringService.PlayerBetPoint(gameModel, playerId);

            gameModel = OptionService.OptionDrawCard(gameModel);

            return gameModel;
        }

    }
}
