﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.BLL.Interface;
using BlackJack.BLL.Helper;
using BlackJack.DAL.Interface;
using BlackJack.Configuration.Constant;
using System.IO;

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

        public async Task<string> UpdateScore(int playerId, int playerCardsValue, int dealerCardsValue, int gameId)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info(StringHelper.PlayerValue(playerId, playerCardsValue, dealerCardsValue));
            if ((playerCardsValue > dealerCardsValue) && (playerCardsValue <= Constant.WinValue))
            {
                await PlayerWinPoints(playerId, gameId);
                await _playerInGameRepository.AnnulBet(playerId, gameId);
                return OptionHelper.OptionWin();
            }

            if ((playerCardsValue <= Constant.WinValue) && (dealerCardsValue > Constant.WinValue))
            {
                await PlayerWinPoints(playerId, gameId);
                await _playerInGameRepository.AnnulBet(playerId, gameId);
                return OptionHelper.OptionWin();
            }

            if (playerCardsValue > Constant.WinValue)
            {
                await PlayerLosePoints(playerId, gameId);
                await _playerInGameRepository.AnnulBet(playerId, gameId);
                return OptionHelper.OptionLose();
            }

            if ((dealerCardsValue > playerCardsValue) && (dealerCardsValue <= Constant.WinValue))
            {
                await PlayerLosePoints(playerId, gameId);
                await _playerInGameRepository.AnnulBet(playerId, gameId);
                return OptionHelper.OptionLose();
            }

            if ((dealerCardsValue == playerCardsValue) && (playerCardsValue <= Constant.WinValue))
            {
                await _playerInGameRepository.AnnulBet(playerId, gameId);
                logger.Info(StringHelper.PlayerDraw(playerId));
                return OptionHelper.OptionDraw();
            }
            return null;
        }

        private async Task PlayerLosePoints(int playerId, int gameId)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            var player = await _playerRepository.GetById(playerId);
            var bet = await _playerInGameRepository.GetBetByPlayerId(playerId, gameId);
            logger.Info(StringHelper.PlayerLose(playerId, bet));
            var newPointsValue = player.Points - bet;
            await _playerRepository.UpdatePoints(playerId, newPointsValue);
        }

        private async Task PlayerWinPoints(int playerId, int gameId)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            var player = await _playerRepository.GetById(playerId);
            var bet = await _playerInGameRepository.GetBetByPlayerId(playerId, gameId);
            logger.Info(StringHelper.PlayerWin(playerId, bet));
            var newPointsValue = player.Points + bet;
            await _playerRepository.UpdatePoints(playerId, newPointsValue);
        }
    }
}
