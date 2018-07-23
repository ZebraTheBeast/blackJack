using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.DAL.Interface
{
    public interface IPlayerInGameRepository
    {
        void AddPlayer(int playerId);
        void RemovePlayer(int playerId);
        void RemoveAll(int playerId);
        List<int> GetAll();
        void MakeBet(int playerId, int bet);
        void AnnulBet(int playerId);
    }
}
