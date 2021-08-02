using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;
using FiveSpnLoggerClientLibrary;
using FiveSpnLoggerClientLibrary.Classes;
using FiveSpnLoggerClientLibrary.Enums;


namespace FiveSpnLoggerServer
{
    public class LoggerServer : BaseScript
    {
        public LoggerServer()
        {
            EventHandlers["ServerBasics:ClientLogMessage"] += new Action<Player,int, string, string>(ReceiveClientLogMessage);
        }

        private void ReceiveClientLogMessage([FromSource] Player player, int severity, string source, string message)
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
                string messageCombined = $"{DateTime.Now,-19} {player.Handle} resource log message. {player.EndPoint} [{(LogMessageSeverity)severity,8}] {source}: {message}";
                Console.WriteLine(messageCombined);
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                ServerLogger.SendServerLogMessage(new LogMessage("Server Logger",LogMessageSeverity.Error,"Exception thrown while attempting to process message from client.\n"+e.Message));
                Console.ForegroundColor = ConsoleColor.White;
            }

        }
    }
}