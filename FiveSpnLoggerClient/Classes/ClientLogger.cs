using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FiveSpnLoggerClient.Enums;

namespace FiveSpnLoggerClient.Classes
{
    public class ClientLogger
    {
        private static readonly ClientLogger _logger = new ClientLogger();
        public static ClientLogger Logger
        {
            get { return _logger; }
        }

        static ClientLogger()
        {

        }
        private ClientLogger()
        {
               
        }

        public static void SendClientLogMessage(LogMessage logMessage)
        {
            string messageCombined = $"{DateTime.Now,-19} [{logMessage.Severity,8}] {logMessage.Source}: {logMessage.Message}";
            Console.WriteLine(messageCombined);
            if (logMessage.Severity == LogMessageSeverity.Error || logMessage.Severity == LogMessageSeverity.Critical)
            {
                BaseScript.TriggerServerEvent("ServerBasics:ClientLogMessage", API.PlayerId(), messageCombined);
            }
        }
    }
}