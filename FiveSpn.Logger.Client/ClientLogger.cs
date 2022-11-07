using System;
using CitizenFX.Core;
using FiveSpn.Logger.Library.Classes;
using FiveSpn.Logger.Library.Enums;

namespace FiveSpn.Logger.Client
{
    public class ClientLogger : BaseScript
    {
        public ClientLogger()
        {
            SendClientLogMessage(new LogMessage("FiveSPN - Logger",LogMessageSeverity.Info,"New resource logger initialized."));
            EventHandlers["FiveSPN-LogToClient"] += new Action<string, int, string>(HandleClientLogMessage);
        }

        private void HandleClientLogMessage(string arg1, int arg2, string arg3)
        {
            SendClientLogMessage(new LogMessage(arg1,(LogMessageSeverity)arg2,arg3));
        }


        private static void SendClientLogMessage(LogMessage logMessage)
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