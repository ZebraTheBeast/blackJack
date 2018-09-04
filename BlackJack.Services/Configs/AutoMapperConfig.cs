using AutoMapper;
using BlackJack.Entities;
using BlackJack.ViewModels;

namespace BlackJack.BusinessLogic.Mappers
{
	public static class AutoMapperConfig
	{
		public static void Initialize()
		{
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<Player, PlayerViewModel>();
				cfg.CreateMap<Player, DealerViewModel>();
			});
		}
	}
}
