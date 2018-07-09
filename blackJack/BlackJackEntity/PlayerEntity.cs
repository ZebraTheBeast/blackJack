﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackJack
{
    public abstract class PlayerEntity
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public Hand Hand { get; set; }
    }
}
