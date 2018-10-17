using AutoMapper;
using BlackJack.BusinessLogic.Helpers;
using BlackJack.Configurations;
using BlackJack.Entities;
using BlackJack.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack.BusinessLogic.Mappers
{
	public class GetGameMapper
	{
		public GetGameGameView GetView(Game game, List<long> deck)
		{
			var getGameViewModel = new GetGameGameView() { Bots = new List<PlayerViewItem>() };

			var human = game.PlayersInGame.Where(player => player.IsHuman == true).FirstOrDefault();
			var dealer = game.PlayersInGame.Where(player => player.Player.Type == Entities.Enums.PlayerType.Dealer).FirstOrDefault();

			getGameViewModel.Human = Mapper.Map<Player, PlayerViewItem>(human.Player);
			getGameViewModel.Human.BetValue = human.BetValue;
			getGameViewModel.Human.Hand = GetHand(game.PlayersInGame.Where(player => player.IsHuman == true).FirstOrDefault().CardsInHand);
			getGameViewModel.Dealer = Mapper.Map<Player, DealerViewItem>(dealer.Player);
			getGameViewModel.Dealer.Hand = GetHand(game.PlayersInGame.Where(player => player.PlayerId == dealer.PlayerId).FirstOrDefault().CardsInHand);

			foreach (var playerInGame in game.PlayersInGame.Where(player => player.Player.Type == Entities.Enums.PlayerType.Bot))
			{
				var bot = new PlayerViewItem();

				bot = Mapper.Map<Player, PlayerViewItem>(playerInGame.Player);
				bot.BetValue = playerInGame.BetValue;

				bot.Hand = GetHand(playerInGame.CardsInHand);

				getGameViewModel.Bots.Add(bot);
			}

			if (getGameViewModel.Human.Hand.CardsInHand.Count() != 0)
			{
				getGameViewModel.Options = StringHelper.OptionDrawCard;
			}

			if ((getGameViewModel.Human.Hand.CardsInHand.Count() == 0)
				|| (getGameViewModel.Human.BetValue == 0))
			{
				getGameViewModel.Options = StringHelper.OptionSetBet(string.Empty);
			}

			getGameViewModel.Deck = deck;

			return getGameViewModel;
		}

		private HandViewItem GetHand(List<Hand> hands)
		{
			var hand = new HandViewItem { CardsInHand = new List<CardViewItem>() };
			if (hands != null)
			{
				foreach (var cardInHand in hands)
				{
					hand.CardsInHand.Add(Mapper.Map<Card, CardViewItem>(cardInHand.Card));
					hand.CardsInHandValue += cardInHand.Card.Value;
				}

				foreach (var card in hand.CardsInHand)
				{
					if ((card.Title.Replace(" ", string.Empty) == Constant.AceCardTitle)
						&& (hand.CardsInHandValue > Constant.WinValue))
					{
						hand.CardsInHandValue -= Constant.ImageCardValue;
					}
				}
			}
			return hand;
		}
	}
}
