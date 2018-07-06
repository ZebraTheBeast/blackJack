using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackJack
{
    public abstract class HandEntity
    {
        public List<CardEntity> _handCard;
        public int _handCardValue;
    }
}
