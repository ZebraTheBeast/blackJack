using BlackJack.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface ICardRepository : IGenericRepository<Card>
	{
		Task<List<Card>> GetCardsById(List<long> cardsId);
		Task PopulateCards(List<Card> cards);
	}
}