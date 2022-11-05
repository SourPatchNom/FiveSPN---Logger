using System;
using System.Globalization;
using System.IO;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FiveSpn.Logger.Library.Classes;
using FiveSpn.Logger.Library.Enums;

namespace FiveSpn.Logger.Server
{
    public class Service : BaseScript
    {
        private readonly LogMessageSeverity _severity = LogMessageSeverity.Debug;
        private readonly string _logPath = API.GetResourcePath(API.GetCurrentResourceName()) + "/FiveSpnLog.txt";
        
        public Service()
        {
            try
            {
                File.Delete(_logPath);
                File.Create(_logPath);
            }
            catch (Exception e)
            {
                WriteMessageToConsole(LogMessageSeverity.Critical,"Logging File Error!\n"+e.Message);
            }
            
            ProcessServerLogMessage(new LogMessage("Server Logger", LogMessageSeverity.Info,"New resource logger initializing."));
            EventHandlers["FiveSPN-ServerLogToServer"] += new Action<string, int, string>(HandleServerLogMessage);
            EventHandlers["FiveSPN-ClientLogToServer"] += new Action<Player, string, int, string>(HandlePlayerLogMessage);
        }

        private void HandlePlayerLogMessage(Player arg1, string arg2, int arg3, string arg4)
        {
            ProcessServerLogMessage(new LogMessage(arg1.Name +" " + arg2, (LogMessageSeverity)arg3, arg4));
        }

        private void HandleServerLogMessage(string arg1, int arg2, string arg3)
        {
            ProcessServerLogMessage(new LogMessage(arg1, (LogMessageSeverity)arg2, arg3));
        }

        private void ProcessServerLogMessage(LogMessage logMessage)
        {
            if (logMessage.Severity > _severity) return;
            var messageCombined = $"FiveSpn>[{logMessage.Source,20}][{logMessage.Severity,8}] {DateTime.Now,-19} : {logMessage.Message}";
            WriteMessageToLogFile(messageCombined);
            WriteMessageToConsole(logMessage.Severity, messageCombined);
        }

        private void WriteMessageToLogFile(string messageCombined)
        {
            try
            {
                using (var writer = File.AppendText(_logPath))
                {
                    writer.WriteLine(messageCombined);
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                WriteMessageToConsole(LogMessageSeverity.Critical,"Logging File Error!\n"+e.Message);
            }
        }

        private void WriteMessageToConsole(LogMessageSeverity severity, string messageCombined)
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
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogMessageSeverity.Debug:
                        Console.ForegroundColor = ConsoleColor.Magenta;
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
                ProcessServerLogMessage(new LogMessage("Server Logger",LogMessageSeverity.Error,"Exception thrown while attempting to process log message from client.\n"+e.Message));
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}