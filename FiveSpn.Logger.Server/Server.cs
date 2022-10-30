using System;
using CitizenFX.Core;
using FiveSpn.Logger.Library.Classes;
using FiveSpn.Logger.Library.Enums;

namespace FiveSpn.Logger.Server
{
    public class Service : BaseScript
    {
        public Service()
        {
            SendServerLogMessage(new LogMessage("Server Logger",LogMessageSeverity.Info,"New resource logger initialized."));
            EventHandlers["FiveSPN-ServerLogToServer"] += new Action<string, int, string>(HandleServerLogMessage);
            EventHandlers["FiveSPN-ClientLogToServer"] += new Action<Player, string, int, string>(HandlePlayerLogMessage);
        }

        private void HandlePlayerLogMessage(Player arg1, string arg2, int arg3, string arg4)
        {
            SendServerLogMessage(new LogMessage(arg1.Name +" " + arg2, (LogMessageSeverity)arg3, arg4));
        }

        private void HandleServerLogMessage(string arg1, int arg2, string arg3)
        {
            SendServerLogMessage(new LogMessage(arg1, (LogMessageSeverity)arg2, arg3));
        }

        public static void SendServerLogMessage(LogMessage logMessage)
        {
            //string messageCombined = $"{DateTime.Now,-19} [{logMessage.Severity,8}] {logMessage.Source}: {logMessage.Message}";
            string messageCombined = $"FiveSpn>[{logMessage.Source,20}][{logMessage.Severity,8}] {DateTime.Now,-19} : {logMessage.Message}";
            WriteMessageToConsole(logMessage.Severity, messageCombined);
        }
        
        private static void WriteMessageToConsole(LogMessageSeverity severity, string messageCombined)
        {
            try
            {
                switch (severity)
                {
                    case LogMessageSeverity.Critical:
                    case LogMessageSeverity.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogMessageSeverity.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogMessageSeverity.Info:
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        break;
                    case LogMessageSeverity.Verbose:
                    case LogMessageSeverity.Debug:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Console.WriteLine(messageCombined);
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                //TODO loop?
                SendServerLogMessage(new LogMessage("Server Logger",LogMessageSeverity.Error,"Exception thrown while attempting to process log message from client.\n"+e.Message));
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}