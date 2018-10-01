using BlackJack.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface ICardProvider
	{
		Task RestoreCardsInDb();
		Task<List<long>> LoadInGameDeck(List<long> cardsInGame);
		Task<List<Card>> GetCardsByIds(List<long> id);
	}
}
