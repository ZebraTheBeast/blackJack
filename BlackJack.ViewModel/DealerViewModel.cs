using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.ViewModel
{
    public class DealerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HandViewModel Hand { get; set; }
    }
}
