using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace BlackJack.Logger
{
    public static class Logger
    {
        public static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        
        public static void Info(string message)
        {
            _logger.Info(message);
        }

        public static void Error(string message)
        {
            _logger.Error(message);
        }
    }
}
