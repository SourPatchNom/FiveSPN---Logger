using System;
using CitizenFX.Core;
using FiveSpnLoggerServerLibrary.Classes;
using FiveSpnLoggerServerLibrary.Enums;

namespace FiveSpnLoggerServerLibrary
{
    public class ServerLogger
    {
        public static ServerLogger Instance { get; } = new ServerLogger();

        static ServerLogger()
        {

        }
        private ServerLogger()
        {
            SendServerLogMessage(new LogMessage("Server Logger",LogMessageSeverity.Info,"New resource logger initialized."));
        }

        public static void SendServerLogMessage(LogMessage logMessage)
        {
            string messageCombined = $"{DateTime.Now,-19} [{logMessage.Severity,8}] {logMessage.Source}: {logMessage.Message}";
            WriteMessageToConsole(logMessage.Severity, messageCombined);
        }

        private static void WriteMessageToConsole(LogMessageSeverity severity, string messageCombined)
        {
            try
            {
                switch ((LogMessageSeverity)severity)
                {
                    case LogMessageSeverity.Critical:
                    case LogMessageSeverity.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogMessageSeverity.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogMessageSeverity.Info:
                        Console.ForegroundColor = ConsoleColor.White;
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
                ServerLogger.SendServerLogMessage(new LogMessage("Server Logger",LogMessageSeverity.Error,"Exception thrown while attempting to process log message from client.\n"+e.Message));
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}