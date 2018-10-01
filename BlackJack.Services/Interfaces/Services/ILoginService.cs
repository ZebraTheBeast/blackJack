using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface ILoginService
	{
		Task<long> StartGame(string playerName, int botsAmount);
		Task<long> LoadGame(string playerName);
	}
}
