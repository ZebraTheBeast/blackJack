using BlackJack.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface ICardProvider
	{
		Task CheckDeck();
		Task<List<int>> LoadDeck(List<int> cardsInGame);
		Task<List<Card>> GetCardsByIds(List<int> id);
	}
}
