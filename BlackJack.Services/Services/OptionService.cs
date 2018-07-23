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
        static public GameModel OptionSetBet(GameModel gameModel)
        {
            gameModel.Options += "Please, make a bet.";
            return gameModel;
        }

        static public GameModel OptionDrawCard(GameModel gameModel)
        {
            gameModel.Options = "Draw card or stand.";
            return gameModel;
        }

        static public GameModel OptionWin(GameModel gameModel)
        {
            gameModel.Options = "You win! ";
            return gameModel;
        }

        static public GameModel OptionLose(GameModel gameModel)
        {
            gameModel.Options = "You lose! ";
            return gameModel;
        }

        static public GameModel OptionDraw(GameModel gameModel)
        {
            gameModel.Options = "You have draw! ";
            return gameModel;
        }
    }
}
