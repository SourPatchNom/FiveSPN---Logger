using System;
using CitizenFX.Core;
using FiveSpnLoggerServerLibrary;
using FiveSpnLoggerServerLibrary.Classes;
using FiveSpnLoggerServerLibrary.Enums;

namespace FiveSpnLoggerClientToServer
{
    public class LoggerClientToServer : BaseScript
    {
        public LoggerClientToServer()
        {
            ServerLogger.SendServerLogMessage(new LogMessage("Server Logger",LogMessageSeverity.Info,"Initializing client to server log message event handler."));
            EventHandlers["ServerBasics:ClientLogMessage"] += new Action<Player,int, string, string>(ReceiveClientLogMessage);
        }

        private void ReceiveClientLogMessage([FromSource] Player player, int severity, string source, string message)
        {
            ServerLogger.SendServerLogMessage(new LogMessage($"{player.EndPoint}:{player.Handle} resource log message. {source}",(LogMessageSeverity)severity,message));
        }
    }
}