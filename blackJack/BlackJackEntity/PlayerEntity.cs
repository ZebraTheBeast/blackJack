using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackJack
{
    public abstract class PlayerEntity
    {
        public string _name { get; set; }
        public int _points { get; set; }
        public Hand _hand { get; set; }
    }
}
