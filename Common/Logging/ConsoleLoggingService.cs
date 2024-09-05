namespace Common.Logging
{
    public class ConsoleLoggingService : ILoggingService
    {
        public void LogError(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        public void LogTask(NetworkingTask serverTask, string? message)
        {
            Console.WriteLine(serverTask.ToString());
            if (message != null)
                Console.WriteLine(message);
        }
    }
}
