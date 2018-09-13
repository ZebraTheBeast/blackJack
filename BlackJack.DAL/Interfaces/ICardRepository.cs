using BlackJack.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Interfaces
{
	public interface ICardRepository
	{
		Task<Card> GetById(int cardId);
		Task FillDB(List<Card> cards);
		Task<IEnumerable<Card>> GetAll();
	}
}
