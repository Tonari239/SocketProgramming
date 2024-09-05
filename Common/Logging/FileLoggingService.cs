namespace Common.Logging
{
    public class FileLoggingService : ILoggingService
    {
        private string logFile;
        public FileLoggingService(string logFileName)
        {
            logFile = logFileName;
        }

        public void LogError(Exception ex)
        {
            using StreamWriter writer = new StreamWriter(logFile);
            writer.WriteLine(ex.Message);
        }

        public void LogTask(NetworkingTask serverTask, string? message)
        {
            using StreamWriter writer = new StreamWriter(logFile);
            writer.WriteLine(serverTask.ToString());
            if (message != null)
                writer.WriteLine(message);
        }
    }
}
