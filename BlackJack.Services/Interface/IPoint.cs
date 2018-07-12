using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.ViewModel;

namespace BlackJack.BLL.Interface
{
    public interface IPoint
    {
        void WinPoints(PlayerModel player);
        void LosePoints(PlayerModel player);
        void AnnulPoints(PlayerModel player);
    }
}
