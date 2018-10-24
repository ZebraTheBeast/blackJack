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
				cfg.CreateMap<GameViewItem, ResponseBetGameView>();
				cfg.CreateMap<GameViewItem, DrawGameView>();
				cfg.CreateMap<GameViewItem, StandGameView>();
                cfg.CreateMap<GameViewItem, ResponseStartGameGameView>();
                cfg.CreateMap<GameViewItem, LoadGameGameView>();
                cfg.CreateMap<StandGameView, GameViewItem>();
			});
		}
	}
}
