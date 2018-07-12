using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlackJack.BLL.Services;
using BlackJack.BLL.Interface;
using Ninject.Modules;

namespace BlackJack.MVC.Util
{
    public class GameModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IGame>().To<Game>();
        }
    }
}