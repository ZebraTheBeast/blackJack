using BlackJack.BLL.Interface;
using BlackJack.BLL.Services;
using Ninject.Modules;

namespace BlackJack.MVC.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IGameHelper>().To<GameHelper>();
        }
    }
}