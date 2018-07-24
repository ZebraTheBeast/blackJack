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
        void AddHuman(int playerId);
        void RemovePlayer(int playerId);
        void RemoveAll();
        List<int> GetAll();
        List<int> GetBots();
        int GetHuman();
        int GetBetByPlayerId(int playerId);
        void MakeBet(int playerId, int bet);
        void AnnulBet(int playerId);
    }
}
