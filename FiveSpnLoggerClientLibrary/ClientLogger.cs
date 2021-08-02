using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FiveSpnLoggerClientLibrary.Classes;
using FiveSpnLoggerClientLibrary.Enums;

namespace FiveSpnLoggerClientLibrary
{
    public class ClientLogger
    {
        public static ClientLogger Logger { get; } = new ClientLogger();

        static ClientLogger()
        {

        }
        private ClientLogger()
        {
            SendClientLogMessage(new LogMessage("FiveSPN - Logger",LogMessageSeverity.Info,"New resource logger initialized."));
        }

        public static void SendClientLogMessage(LogMessage logMessage)
        {
            try
            {
                Debug.WriteLine($"{DateTime.Now,-19} [{logMessage.Severity,8}] {logMessage.Source}: {logMessage.Message}");
                if (logMessage.Severity == LogMessageSeverity.Error || logMessage.Severity == LogMessageSeverity.Critical)
                {
                    BaseScript.TriggerServerEvent("ServerBasics:ClientLogMessage", logMessage.Severity, logMessage.Source, logMessage.Message);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{DateTime.Now,-19} [{LogMessageSeverity.Error,8}] Client Logger : Exception thrown attempting to log a message!");
                Debug.WriteLine(e.Message);
            }
        }
    }
}