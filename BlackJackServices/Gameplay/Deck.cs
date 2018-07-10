﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;
using BlackJack.Configuration.Constant;
using BlackJack.Entity.Enum;

namespace BlackJack.Services.Gameplay
{
    public static class Deck
    {
        private static Random _rng = new Random();

        public static void GiveCard(Player player, List<Card> deck)
        {
            player.Hand.CardList.Add(deck[0]);
            deck.Remove(deck[0]);
            Hand.CountHandValue(player);
        }

        public static void ShuffleDeck(List<Card> deck)
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
