using AutoMapper;
using BlackJack.BusinessLogic.Helpers;
using BlackJack.Configurations;
using BlackJack.Entities;
using BlackJack.Entities.Enums;
using BlackJack.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack.BusinessLogic.Mappers
{
    public class GameMapper
    {
        public GetGameGameView GetView(Game game, List<PlayerInGame> playersInGame, List<long> deck)
        {
            foreach (var playerInGame in playersInGame)
            {
                game.PlayersInGame.Where(player => player.PlayerId == playerInGame.PlayerId).FirstOrDefault().CardsInHand = playerInGame.CardsInHand;
            }

            var getGameViewModel = new GetGameGameView() { Bots = new List<PlayerViewItem>() };

            var human = game.PlayersInGame.Where(player => player.Player.Type == PlayerType.Human).FirstOrDefault();
            var dealer = game.PlayersInGame.Where(player => player.Player.Type == PlayerType.Dealer).FirstOrDefault();

            getGameViewModel.Human = Mapper.Map<Player, PlayerViewItem>(human.Player);
            getGameViewModel.Human.BetValue = human.BetValue;
            getGameViewModel.Human.Hand = GetHand(human.CardsInHand);
            getGameViewModel.Dealer = Mapper.Map<Player, DealerViewItem>(dealer.Player);
            getGameViewModel.Dealer.Hand = GetHand(game.PlayersInGame.Where(player => player.PlayerId == dealer.PlayerId).FirstOrDefault().CardsInHand);

            foreach (var playerInGame in game.PlayersInGame.Where(player => player.Player.Type == PlayerType.Bot))
            {
                var bot = new PlayerViewItem();

                bot = Mapper.Map<Player, PlayerViewItem>(playerInGame.Player);
                bot.BetValue = playerInGame.BetValue;

                bot.Hand = GetHand(playerInGame.CardsInHand);

                getGameViewModel.Bots.Add(bot);
            }

            if (getGameViewModel.Human.Hand.CardsInHand.Count() != 0)
            {
                getGameViewModel.Options = UserMessages.OptionDrawCard;
            }

            if ((getGameViewModel.Human.Hand.CardsInHand.Count() == 0)
                || (getGameViewModel.Human.BetValue == 0))
            {
                getGameViewModel.Options = StringHelper.OptionSetBet(string.Empty);
            }

            getGameViewModel.Deck = deck;

            return getGameViewModel;
        }

        private HandViewItem GetHand(List<Card> cardsInHand)
        {
            var hand = new HandViewItem { CardsInHand = new List<CardViewItem>() };

            if (cardsInHand == null)
            {
                return hand;
            }

            foreach (var cardInHand in cardsInHand)
            {
                hand.CardsInHand.Add(Mapper.Map<Card, CardViewItem>(cardInHand));
                hand.CardsInHandValue += cardInHand.Value;
            }

            foreach (var card in hand.CardsInHand)
            {
                if ((card.Title.Replace(" ", string.Empty) == Constant.AceCardTitle)
                    && (hand.CardsInHandValue > Constant.WinValue))
                {
                    hand.CardsInHandValue -= Constant.ImageCardValue;
                }
            }

            return hand;
        }
    }
}
