using AutoMapper;
using BlackJack.Entities;
using BlackJack.ViewModels;
using System.Collections.Generic;

namespace BlackJack.BusinessLogic.Mappers
{
	public static class AutoMapperConfig
	{
		public static void Initialize()
		{
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<Player, PlayerViewModelItem>();
				cfg.CreateMap<Player, DealerViewModelItem>();
				cfg.CreateMap<Card, CardViewModelItem>();
				cfg.CreateMap<GetGameViewModel, ResponseBetGameViewModel>();
				cfg.CreateMap<GetGameViewModel, DrawGameViewModel>();
				cfg.CreateMap<GetGameViewModel, StandGameViewModel>();
				cfg.CreateMap<StandGameViewModel, GetGameViewModel>();
			});
		}
	}
}
