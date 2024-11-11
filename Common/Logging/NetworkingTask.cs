using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
     public enum Action
    {
        SERVER_STARTED,
        CLIENT_CONNECTED,
        CLIENT_DISCONNECTED,
        RECEIVED_DATA,
        SENT_DATA
    }

    public class NetworkingTask
    {
        public DateTime ExecutionTimeStamp { get; }
        public Action ServerAction { get; set; }

        public NetworkingTask(DateTime executionTimeStamp, Action serverAction)
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
