using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Logging
{
    public interface ILoggingService
    {

        public void LogError(Exception ex);
        public void LogTask(ServerTask serverTask);
    }
}
