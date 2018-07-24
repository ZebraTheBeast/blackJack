using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.BLL.Services
{
    static public class OptionService
    {
        static public GameViewModel OptionSetBet(GameViewModel gameModel)
        {
            gameModel.Options += "Please, make a bet.";
            return gameModel;
        }

        static public GameViewModel OptionDrawCard(GameViewModel gameModel)
        {
            gameModel.Options = "Draw card or stand.";
            return gameModel;
        }

        static public GameViewModel OptionWin(GameViewModel gameModel)
        {
            gameModel.Options = "You win! ";
            return gameModel;
        }

        static public GameViewModel OptionLose(GameViewModel gameModel)
        {
            gameModel.Options = "You lose! ";
            return gameModel;
        }

        static public GameViewModel OptionDraw(GameViewModel gameModel)
        {
            gameModel.Options = "You have draw! ";
            return gameModel;
        }

        static public GameViewModel OptionRefreshGame(GameViewModel gameModel)
        {
            gameModel.Options += "You haven't enough point! Please, refresh the game!";
            return gameModel;
        }
    }
}
