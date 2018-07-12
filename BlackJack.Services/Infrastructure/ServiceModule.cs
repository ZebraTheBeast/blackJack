using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using BlackJack.DAL.Repository;
using BlackJack.DAL.Interface;

namespace BlackJack.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string _connectionString;
        public ServiceModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(_connectionString);
        }
    }
}
