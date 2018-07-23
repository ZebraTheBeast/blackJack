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

        public static GameViewModel Dealing(GameViewModel gameModel)
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

        public static GameViewModel GiveCard(int playerId, GameViewModel gameModel)
        {
            var index = gameModel.Players.FindIndex(p => p.Id == playerId);
            StringService.PlayerDrawCard(gameModel, index);
            gameModel.Players.Find(p => p.Id == playerId).Hand.CardList.Add(gameModel.Deck[0]);
            gameModel.Deck.Remove(gameModel.Deck[0]);
            gameModel.Players[index] = CountPlayerCardsValue(gameModel.Players[index]);

            return gameModel;
        }

        public static GameViewModel StartGame(string playerName)
        {
            var players = new List<PlayerViewModel>();
            var gameModel = new GameViewModel();

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Player, PlayerViewModel>());

            var playerEntity = _playerRepository.GetByName(playerName);
            var humanPlayer = Mapper.Map<Player, PlayerViewModel>(playerEntity);
            var playersEntity = _playerRepository.GetBots();

            foreach (var item in playersEntity)
            {
                players.Add(Mapper.Map<Player, PlayerViewModel>(item));
            }

            players.Add(humanPlayer);

            for (var i = 0; i < players.Count; i++)
            {
                players[i].Hand = new HandViewModel { CardList = new List<CardViewModel>() };
            }

            gameModel.Players = players;
            gameModel.GameStats = new List<string>();
            gameModel.Deck = new List<CardViewModel>();
            gameModel = OptionService.OptionSetBet(gameModel);

            return gameModel;
        }

        private static PlayerViewModel CountPlayerCardsValue(PlayerViewModel player)
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

        public static GameViewModel BotTurn(GameViewModel gameModel, PlayerViewModel player, int minValue)
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

        public static GameViewModel EditPoints(GameViewModel gameModel)
        {
            for (var i = 1; i < gameModel.Players.Count; i++)
            {
                gameModel = PointCheckerService.CheckPlayerWithDealer(gameModel.Players[i], gameModel.Players[0], gameModel);
                _playerRepository.Update(Mapper.Map<PlayerViewModel, Player>(gameModel.Players[i]));
            }

            if (gameModel.Players.Last().Points < Constant.MinPointsValueToPlay)
            {
                gameModel = OptionService.OptionRefreshGame(gameModel);
            }
            if (gameModel.Players.Last().Points >= Constant.MinPointsValueToPlay)
            {
                gameModel = OptionService.OptionSetBet(gameModel);
            }
            return gameModel;
        }

        public static GameViewModel EndTurn(GameViewModel gameModel)
        {
            for (var i = 0; i < gameModel.Players.Count; i++)
            {
                gameModel.Players[i].Hand.CardList = new List<CardViewModel>();
                gameModel.Players[i] = CountPlayerCardsValue(gameModel.Players[i]);
            }

            gameModel.Deck = new List<CardViewModel>();

            return gameModel;
        }

        public static GameViewModel PlaceBet(GameViewModel gameModel, int playerId, int pointsValue)
        {
            gameModel.Players.Find(p => p.Id == playerId).Hand.Points = pointsValue;
            gameModel = StringService.PlayerBetPoint(gameModel, playerId);

            gameModel = OptionService.OptionDrawCard(gameModel);

            return gameModel;
        }

    }
}
