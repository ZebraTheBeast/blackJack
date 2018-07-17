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
             
            foreach (var player in gameModel.Players)
            {
                gameModel = GiveCard(player.Id, gameModel);
                gameModel = GiveCard(player.Id, gameModel);
            }
            
            return gameModel;
        }

        public GameModel GiveCard(int playerId, GameModel gameModel)
        {
            gameModel.Players.Find(p => p.Id == playerId).Hand.CardList.Add(gameModel.Deck[0]);
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
                new PlayerModel { Id = 0, Name = "Dealer", Hand = new HandModel(){ CardList = new List<CardModel>() }, Points = 10000 },
                new PlayerModel { Id = 1, Name = "Хена", Hand = new HandModel(){ CardList = new List<CardModel>() }, Points = 1000 },
                new PlayerModel { Id = 2, Name = "Ихарь", Hand = new HandModel(){ CardList = new List<CardModel>() }, Points = 1000 },
                new PlayerModel { Id = 3, Name = "Ондрей", Hand = new HandModel(){ CardList = new List<CardModel>() }, Points = 1000 }
            };
            player.Hand = new HandModel();
            player.Hand.CardList = new List<CardModel>();
            gameModel.Players = players;
            gameModel.Players.Add(player);

            return gameModel;
        }
    }
}
