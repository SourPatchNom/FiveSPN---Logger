using System;
using FiveSpnLoggerClientLibrary.Classes;
using FiveSpnLoggerClientLibrary.Enums;

namespace FiveSpnLoggerClientLibrary
{
    public class ServerLogger
    {
        private static readonly ServerLogger _instance = new ServerLogger();
        public static ServerLogger Instance
        {
            get { return _instance; }
        }

        static ServerLogger()
        {

        }
        private ServerLogger()
        {
            SendServerLogMessage(new LogMessage("FiveSPN - Logger",LogMessageSeverity.Info,"Logger initialized."));
        }

        public static void SendServerLogMessage(LogMessage logMessage)
        {
            string messageCombined = $"{DateTime.Now,-19} [{logMessage.Severity,8}] {logMessage.Source}: {logMessage.Message}";
            Console.WriteLine(messageCombined);
            // if (logMessage.Severity == LogMessageSeverity.Error || logMessage.Severity == LogMessageSeverity.Critical)
            // {
            //     BaseScript.TriggerServerEvent("ServerBasics:ClientLogMessage", API.PlayerId(), messageCombined);
            // }
        }
    }
}