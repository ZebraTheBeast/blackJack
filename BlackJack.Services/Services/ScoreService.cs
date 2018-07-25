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
    public class ScoreService 
    {
        IPlayerInGameRepository _playerInGameRepository;
        IPlayerRepository _playerRepository;

        public ScoreService(IPlayerInGameRepository playerInGameRepository, IPlayerRepository playerRepository)
        {
            _playerInGameRepository = playerInGameRepository;
            _playerRepository = playerRepository;
        }

        public string UpdateScore(int playerId, int playerCardsValue, int dealerCardsValue)
        {
            if ((playerCardsValue > dealerCardsValue) && (playerCardsValue <= Constant.WinValue))
            {
                PlayerWinPoints(playerId);
                _playerInGameRepository.AnnulBet(playerId);
                return OptionHelper.OptionWin();
            }

            if ((playerCardsValue <= Constant.WinValue) && (dealerCardsValue > Constant.WinValue))
            {
                PlayerWinPoints(playerId);
                _playerInGameRepository.AnnulBet(playerId);
                return OptionHelper.OptionWin();
            }

            if (playerCardsValue > Constant.WinValue)
            {
                PlayerLosePoints(playerId);
                _playerInGameRepository.AnnulBet(playerId);
                return OptionHelper.OptionLose();
            }

            if ((dealerCardsValue > playerCardsValue) && (dealerCardsValue <= Constant.WinValue))
            {
                PlayerLosePoints(playerId);
                _playerInGameRepository.AnnulBet(playerId);
                return OptionHelper.OptionLose();
            }

            if ((dealerCardsValue == playerCardsValue) && (playerCardsValue <= Constant.WinValue))
            {
                _playerInGameRepository.AnnulBet(playerId);
                return OptionHelper.OptionDraw();
            }

            return "Error!";
        }

        private void PlayerLosePoints(int playerId)
        {
            var player = _playerRepository.GetById(playerId);
            var bet = _playerInGameRepository.GetBetByPlayerId(playerId);
            var newPointsValue = player.Points - bet;
            _playerRepository.UpdatePoints(playerId, newPointsValue);
        }

        private void PlayerWinPoints(int playerId)
        {
            var player = _playerRepository.GetById(playerId);
            var bet = _playerInGameRepository.GetBetByPlayerId(playerId);
            var newPointsValue = player.Points + bet;
            _playerRepository.UpdatePoints(playerId, newPointsValue);
        }
    }
}
