using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;
using BlackJack.Configuration.Constant;
using BlackJack.Entity.Enum;


namespace BlackJack.Services.Gameplay
{
    public static class Deck
    {
        private static Random _rng = new Random();

        // TODO - подумать нужно оно тут или сделать у игрока DrawCard
        public static void GiveCard(Player player, InGame inGame)
        {
            player.Hand.CardList.Add(inGame.Deck[0]);
            inGame.Deck.Remove(inGame.Deck[0]);
            player.Hand.CardListValue = Hand.GetHandValue(player);
            inGame.Players.Find(x => x.Id == player.Id).Hand = player.Hand;
        }

        // TODO - подумать нужно оно тут или сделать в игроке DrawCard, если нет, то какой вариант лучше
        public static InGame GiveCard2(Player player, List<Card> deck, List<Player> players)
        {
            InGame inGame = new InGame();

            player.Hand.CardList.Add(deck[0]);
            deck.Remove(deck[0]);
            player.Hand.CardListValue = Hand.GetHandValue(player);
            players.Find(x => x.Id == player.Id).Hand = player.Hand;

            inGame.Deck = deck;
            inGame.Players = players;

            return inGame;
        }
        
        public static List<Card> ShuffleDeck(List<Card> deck)
        {
            deck = GetNewDeck();
            int n = deck.Count;

            while (n > 1)
            {
                n--;
                int k = _rng.Next(n + 1);
                Card value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
            return deck;
        }

        public static void FillDeckWithCard(List<Card> deck, int end, List<string> cardNames, List<int> cardValues)
        {
            int cardColorValue = 0;
            int cardTitleValue = 0;
            int cardColorSize = Enum.GetNames(typeof(CardColor)).Length - 1;

            for (int i = 0; i < end; i++)
            {
                var card = new Card();

                card.Title = cardNames[cardTitleValue];
                card.Value = cardValues[cardTitleValue];
                card.Color = (CardColor)cardColorValue++;

                deck.Add(card);

                if (cardColorValue > cardColorSize)
                {
                    cardColorValue = 0;
                    cardTitleValue++;
                }
            }
        }

        public static List<Card> GetNewDeck()
        {
            var deck = new List<Card>();
            var valueList = Enumerable.Range(Constant.NumberStartCard, Constant.CountNumberCard).ToList();
            var titleList = valueList.ConvertAll<string>(delegate (int i) { return i.ToString(); });

            foreach (var value in Enum.GetNames(typeof(CardTitle)))
            {
                titleList.Add(value);
            }

            foreach (var value in Enum.GetValues(typeof(CardTitle)))
            {
                valueList.Add((int)value);
            }

            FillDeckWithCard(deck, Constant.DeckSize, titleList, valueList);

            return deck;
        }
    }
}
