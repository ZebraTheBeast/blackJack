using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackJack
{
    public class Gameplay
    {
        // nado li sozdavat GameEntity i tuda zasunut svoistva _deck, _players?
        public Deck _deck;
        public List<Player> _players = new List<Player>();

        public Gameplay()
        {
            _deck = new Deck();
        }

       
        public void PlayerAdd(Player player)
        {
            _players.Add(player);
        }

        
        public void Dealing()
        {
            foreach (Player player in _players)
            {
                _deck.GiveCard(player);
                _deck.GiveCard(player);
            }
        }

       //TODO - v UI
        public void ShowMenu()
        {
            Console.WriteLine("Choose:");
            Console.WriteLine("\tEnter to Hit");
            Console.WriteLine("\tEsc to Stand");
        }
       
        public void Turn(Player player)
        {
            ConsoleKeyInfo consoleKeyInfo;
            bool continueTurn = true;
            do
            {
                //ShowMenu() ??
                consoleKeyInfo = Console.ReadKey();
                if(consoleKeyInfo.Key == ConsoleKey.Enter)
                {
                    _deck.GiveCard(player);
                }
                if (consoleKeyInfo.Key == ConsoleKey.Escape)
                {
                    continueTurn = false;
                }
                if(player._hand._handCardValue > 21)
                {
                    continueTurn = false;
                }

            } while (continueTurn);
        }
        
        public void play()
        {
            
            //sdelat stavku
            //razdali karty
            //hod 
            //pereshet groshey
        }
    }
}
