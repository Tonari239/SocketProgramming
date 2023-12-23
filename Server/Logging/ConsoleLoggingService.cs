using Server.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ConsoleLoggingService : ILoggingService
    {
        public void LogError(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        public void LogTask(ServerTask serverTask)
        {
            Console.WriteLine(serverTask.ToString());
        }
    }
}
