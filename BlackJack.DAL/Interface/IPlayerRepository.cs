using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Entity;
namespace BlackJack.DAL.Interface
{
    public interface IPlayerRepository
    {
        Player GetByName(string Name);
        IEnumerable<Player> GetBots();
        Player Create(Player player);
        void UpdatePoints(int playerId, int newPointsValue);
        void RestorePoints(int playerId);
    }
}
