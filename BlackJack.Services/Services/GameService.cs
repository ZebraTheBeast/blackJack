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

        public LoginPlayersModel GetStartPlayers()
        {
            LoginPlayersModel loginPlayerModel = new LoginPlayersModel();
            loginPlayerModel.PlayerList.Add(new PlayerModel { Id = 0, Name = "Dealer", Hand = new HandModel(), Points = 10000 });
            return loginPlayerModel;
        }

        public GameModel Dealing()
        {
            GameModel gameModel = new GameModel();
            _cardList = DeckService.GetShuffledDeck();

            foreach (var player in _playerList)
            {
                GiveCard(player);
                GiveCard(player);
            }

            gameModel.Players = _playerList;
            gameModel.Deck = _cardList;
            return gameModel;
        }

        public void GiveCard(PlayerModel player)
        {
            _playerList.First(p => p.Id == player.Id).Hand.CardList.Add(_cardList[0]);
            _cardList.Remove(_cardList[0]);
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


        public GameModel PlayerTest()
        {
            var gameModel = new GameModel();

            var players = new List<PlayerModel>
            {
                new PlayerModel { Id = 0, Name = "Dealer", Hand = new HandModel(), Points = 10000 },
                new PlayerModel { Id = 1, Name = "Хена", Hand = new HandModel(), Points = 1000 },
                new PlayerModel { Id = 2, Name = "Ихарь", Hand = new HandModel(), Points = 1000 },
                new PlayerModel { Id = 3, Name = "Ондрей", Hand = new HandModel(), Points = 1000 }
            };
            _playerList = players;

            gameModel.Players = _playerList;
            return gameModel;
        }

        public GameModel PullCard(int id)
        {
            var gameModel = new GameModel();
            var player = _playerList.Find(p => p.Id == id);
            GiveCard(player);

            gameModel.Players = _playerList;
            gameModel.Deck = _cardList;
            return gameModel;
        }
    }
}
