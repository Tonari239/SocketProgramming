using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Logging
{
    public enum ServerAction
    {
           SERVER_STARTED,
           CLIENT_CONNECTED,
           CLIENT_DISCONNECTED,
           RECEIVED_DATA,
           SEND_DATA
    }

    public class ServerTask
    {
        public DateTime ExecutionTimeStamp { get; }
        public ServerAction ServerAction { get; set; }

        public ServerTask(DateTime executionTimeStamp, ServerAction serverAction)
        {
            ExecutionTimeStamp = executionTimeStamp;
            ServerAction = serverAction;
        }

        public override string ToString()
        {
            return $"Server executed task {ServerAction.ToString()} at time {ExecutionTimeStamp}";
        }
    }
}
