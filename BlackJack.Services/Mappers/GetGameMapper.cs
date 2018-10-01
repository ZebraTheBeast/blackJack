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
		public GetGameViewModel GetViewModel(Game game, List<long> deck)
		{
			var getGameViewModel = new GetGameViewModel();

			var human = game.PlayersInGame.Where(player => player.IsHuman == true).FirstOrDefault();
			var dealer = game.PlayersInGame.Where(player => player.Player.Type == Entities.Enums.PlayerType.Dealer).FirstOrDefault();

			getGameViewModel.Human = Mapper.Map<Player, PlayerViewModelItem>(human.Player);
			getGameViewModel.Human.BetValue = human.BetValue;
			getGameViewModel.Human.Hand = GetHand(game.PlayersInGame.Where(player => player.IsHuman == true).FirstOrDefault().CardsInHand);
			getGameViewModel.Dealer.Hand = GetHand(game.PlayersInGame.Where(player => player.PlayerId == dealer.Id).FirstOrDefault().CardsInHand);

			foreach (var playerInGame in game.PlayersInGame.Where(player => player.Player.Type == Entities.Enums.PlayerType.Bot))
			{
				var bot = new PlayerViewModelItem();

				bot = Mapper.Map<Player, PlayerViewModelItem>(playerInGame.Player);
				bot.BetValue = playerInGame.BetValue;

				bot.Hand = GetHand(playerInGame.CardsInHand);
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

		private HandViewModelItem GetHand(List<Hand> hands)
		{
			var hand = new HandViewModelItem { CardsInHand = new List<CardViewModelItem>() };

			foreach (var cardInHand in hands)
			{
				hand.CardsInHand.Add(Mapper.Map<Card, CardViewModelItem>(cardInHand.Card));
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

			return hand;
		}
	}
}
