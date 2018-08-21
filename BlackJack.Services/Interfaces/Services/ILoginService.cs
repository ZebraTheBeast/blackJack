using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Interfaces
{
	public interface ILoginService
	{
		Task<int> StartGame(string playerName);
		Task<int> LoadGame(string playerName);
	}
}
