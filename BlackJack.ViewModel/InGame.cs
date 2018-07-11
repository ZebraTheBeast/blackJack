﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class InGame
    {
        public List<PlayerModel> Players { get; set; }
        public List<CardModel> Deck { get; set; }

        public InGame()
        {
            Players = new List<PlayerModel>();
            Deck = new List<CardModel>();
        }
    }
}
