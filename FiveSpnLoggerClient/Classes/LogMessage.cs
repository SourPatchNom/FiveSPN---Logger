using FiveSpnLoggerClient.Enums;

namespace FiveSpnLoggerClient.Classes
{
    public class LogMessage
    {
        public string Source;
        public LogMessageSeverity Severity;
        public string Message;

        public LogMessage(string source, LogMessageSeverity severity, string message)
        {
            Source = source;
            Severity = severity;
            Message = message;
        }
    }
}