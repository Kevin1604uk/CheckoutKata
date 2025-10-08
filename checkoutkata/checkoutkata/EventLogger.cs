using System;
using System.Diagnostics;

namespace checkoutkata
{
    public class EventLogger : ILogger
    {
        public event EventHandler<string>? LogWritten;

        private readonly string _source = "CheckoutKataApp";
        private readonly string _logName = "Application";

        public EventLogger()
        {
            if (!EventLog.SourceExists(_source))
            {
                EventLog.CreateEventSource(_source, _logName);
            }
        }

        public void LogError(string message)
        {
            EventLog.WriteEntry(_source, message, EventLogEntryType.Error);
            LogWritten?.Invoke(this, message);
        }
    }
}