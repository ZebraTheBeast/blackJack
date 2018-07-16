using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.DAL.Interface;
using BlackJack.BLL.Interface;

namespace BlackJack.BLL.Services
{
    public class GameService : IGame
    {
        public List<PlayerModel> _playerList;
        public List<CardModel> _cardList;
        
        // TODO - peredavat to sho vozvrashaesh

        public GameService()
        {
            _playerList = new List<PlayerModel>();
            _cardList = new List<CardModel>();
        }

        public GameModel Dealing(GameModel gameModel)
        {
            gameModel.Deck = DeckService.GetShuffledDeck();
            и 
            foreach (var player in gameModel.Players)
            {
                gameModel = GiveCard(player, gameModel);
                gameModel = GiveCard(player, gameModel);
            }
            
            return gameModel;
        }

        public GameModel GiveCard(PlayerModel player, GameModel gameModel)
        {
            //gameModel.Players.First(p => p.Id == player.Id).Hand.CardList.Add(gameModel.Deck[0]);
            gameModel.Players.Find(p => p == player).Hand.CardList.Add(gameModel.Deck[0]);
            gameModel.Deck.Remove(gameModel.Deck[0]);
            return gameModel;
        }

        public GameModel AddPlayers(string name)
        {
            var gameModel = new GameModel();

            //foreach (var player in playerList)
            //{
            //    _playerList.Add(player);
            //}

            _playerList.Add(new PlayerModel() { Id = 0, Name = name, Hand = new HandModel(), Points = 333 });

            gameModel.Players = _playerList;
            return gameModel;
        }


        public GameModel PlayerTest(GameModel gameModel, PlayerModel player)
        {
           var players = new List<PlayerModel>
            {
                new PlayerModel { Id = 0, Name = "Dealer", Hand = new HandModel(), Points = 10000 },
                new PlayerModel { Id = 1, Name = "Хена", Hand = new HandModel(), Points = 1000 },
                new PlayerModel { Id = 2, Name = "Ихарь", Hand = new HandModel(), Points = 1000 },
                new PlayerModel { Id = 3, Name = "Ондрей", Hand = new HandModel(), Points = 1000 }
            };

            gameModel.Players = players;
            gameModel.Players.Add(player);

            return gameModel;
        }

        public GameModel PullCard(int id)
        {
            var gameModel = new GameModel();
            var player = _playerList.Find(p => p.Id == id);
            //GiveCard(player);

            gameModel.Players = _playerList;
            gameModel.Deck = _cardList;
            return gameModel;
        }
    }
}
