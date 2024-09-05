namespace Common.Logging
{
    public interface ILoggingService
    {

        public void LogError(Exception ex);
        public void LogTask(NetworkingTask serverTask, string? message = "");
    }
}
