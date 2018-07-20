﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class HandViewModel
    {
        public List<CardViewModel> CardList { get; set; }
        public int CardListValue { get; set; }
        public int Points { get; set; }
    }
}
