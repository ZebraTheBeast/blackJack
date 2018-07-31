using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Configuration.Constant;
using BlackJack.Entity.Enum;
using BlackJack.Entity;
using BlackJack.DAL.Interface;
using BlackJack.DAL.Repository;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Helper;
using NLog;
using System.IO;

namespace BlackJack.BLL.Services
{
    public class DeckService : IDeckService
    {
        IHandRepository _handRepository;
        IPlayerRepository _playerRepository;

        public DeckService( IHandRepository handRepository, IPlayerRepository playerRepository)
        {
            _handRepository = handRepository;
            _playerRepository = playerRepository;

            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\"));
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(path + "BlackJack.Configuration\\Nlog.config", true);
        }

        public List<int> RefreshAndShuffleDeck()
        {
            Random rng = new Random();
            var deck = new List<Card>();
            var cardIdList = new List<int>();
            deck = CardHelper.GetFullDeck();

            int n = deck.Count;

            for (var i = 0; i < n; i++)
            {
                int k = rng.Next(n);
                var value = deck[i];
                deck[i] = deck[k];
                deck[k] = value;
            }

            foreach (var card in deck)
            {
                cardIdList.Add(card.Id);
            }
      

            return cardIdList;
        }

        public async Task GiveCardFromDeck(int playerId, int cardId)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                var player = _playerRepository.GetById(playerId);
                if(player == null)
                {
                    throw new Exception("Player doesn't exist");
                }

                await _handRepository.GiveCardToPlayer(playerId, cardId);   
            }
            catch(Exception exception)
            {
                logger.Error($"{exception.Message}");
            }
        }

       

    }
}
