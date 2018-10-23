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
				cfg.CreateMap<Player, PlayerViewItem>();
				cfg.CreateMap<Player, DealerViewItem>();
				cfg.CreateMap<Card, CardViewItem>();
				cfg.CreateMap<GetGameGameView, ResponseBetGameView>();
				cfg.CreateMap<GetGameGameView, DrawGameView>();
				cfg.CreateMap<GetGameGameView, StandGameView>();
                cfg.CreateMap<GetGameGameView, ResponseStartGameGameView>();
                cfg.CreateMap<GetGameGameView, LoadGameGameView>();
                cfg.CreateMap<StandGameView, GetGameGameView>();
			});
		}
	}
}
