using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FiveSpn.Logger.Client.Classes;
using FiveSpn.Logger.Client.Enums;

namespace FiveSpn.Logger.Client
{
    public class ClientLogger : BaseScript
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
                Debug.WriteLine($"[{logMessage.Source,20}][{logMessage.Severity,8}] {DateTime.Now,-19} : {logMessage.Message}");
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