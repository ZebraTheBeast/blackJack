using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Helper;
using BlackJack.DAL.Interface;
using BlackJack.Configuration.Constant;

namespace BlackJack.BLL.Services
{
    public class ScoreService : IScoreService
    {
        IPlayerInGameRepository _playerInGameRepository;
        IPlayerRepository _playerRepository;

        public ScoreService(IPlayerInGameRepository playerInGameRepository, IPlayerRepository playerRepository)
        {
            _playerInGameRepository = playerInGameRepository;
            _playerRepository = playerRepository;
        }

        public async Task<string> UpdateScore(int playerId, int playerCardsValue, int dealerCardsValue)
        {
            if ((playerCardsValue > dealerCardsValue) && (playerCardsValue <= Constant.WinValue))
            {
                await PlayerWinPoints(playerId);
                await _playerInGameRepository.AnnulBet(playerId);
               
                return OptionHelper.OptionWin();
            }

            if ((playerCardsValue <= Constant.WinValue) && (dealerCardsValue > Constant.WinValue))
            {
                await PlayerWinPoints(playerId);
                await _playerInGameRepository.AnnulBet(playerId);
               
                return OptionHelper.OptionWin();
            }

            if (playerCardsValue > Constant.WinValue)
            {
                await PlayerLosePoints(playerId);
                await _playerInGameRepository.AnnulBet(playerId);
            }

            if ((dealerCardsValue > playerCardsValue) && (dealerCardsValue <= Constant.WinValue))
            {
                await PlayerLosePoints(playerId);
                await _playerInGameRepository.AnnulBet(playerId);
                
                return OptionHelper.OptionLose();
            }

            if ((dealerCardsValue == playerCardsValue) && (playerCardsValue <= Constant.WinValue))
            {
                await _playerInGameRepository.AnnulBet(playerId);
                
                return OptionHelper.OptionDraw();
            }

            return "Error!";
        }

        private async Task PlayerLosePoints(int playerId)
        {
            var player = await _playerRepository.GetById(playerId);
            var bet = await _playerInGameRepository.GetBetByPlayerId(playerId);
            var newPointsValue = player.Points - bet;
            await _playerRepository.UpdatePoints(playerId, newPointsValue);
        }

        private async Task PlayerWinPoints(int playerId)
        {
            var player = await _playerRepository.GetById(playerId);
            var bet = await _playerInGameRepository.GetBetByPlayerId(playerId);
            var newPointsValue = player.Points + bet;
            await _playerRepository.UpdatePoints(playerId, newPointsValue);
        }
    }
}
