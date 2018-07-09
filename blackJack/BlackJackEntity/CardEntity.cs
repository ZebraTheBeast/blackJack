using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackJack
{
    //еnum в отдельные файлы, 1 enum - 1 класс
    //создать сервисы, а туда все переменные
    //изменить создание карт, с 2 по 10
    //хуйня с наследованиями
    public enum Color
    {
        Diamonds = 0,
        Hearts = 1,
        Spades = 2,
        Clubs = 3
    }
    
    public enum Title
    {
       Jack = 0, Queen = 1, King = 2, Ace = 11
    }
    public class CardEntity
    {
        public Title Title { get; set; }
        public int Value { get; set; }
        public Color CardColor { get; set; }
    }
}
