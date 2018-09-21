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
				cfg.CreateMap<Player, PlayerGetGameViewModelItem>();
				cfg.CreateMap<Player, DealerGetGameViewModelItem>();
				cfg.CreateMap<Card, CardGetGameViewModelItem>();
				cfg.CreateMap<GetGameViewModel, ResponseBetGameViewModel>();
				cfg.CreateMap<GetGameViewModel, DrawGameViewModel>();
				cfg.CreateMap<GetGameViewModel, StandGameViewModel>();
				cfg.CreateMap<StandGameViewModel, GetGameViewModel>();
			});
		}
	}
}
