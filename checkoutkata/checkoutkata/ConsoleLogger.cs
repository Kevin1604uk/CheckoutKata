namespace checkoutkata
{
    public class ConsoleLogger : ILogger
    {
        public event EventHandler<string>? LogWritten;

        public void LogError(string message) => Console.Error.WriteLine(String.Format(MessageHelpers.Error, message ?? ""));

    }
}
