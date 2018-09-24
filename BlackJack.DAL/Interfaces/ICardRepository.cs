using BlackJack.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface ICardRepository
	{
		Task<List<Card>> GetCardsById(List<int> cardsId);
		Task PopulateCards(List<Card> cards);
		Task<List<Card>> GetAll();
		Task DeleteAll();
	}
}
