using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FiveSpn.Logger.Library.Classes;
using FiveSpn.Logger.Library.Enums;
using Enumerable = System.Linq.Enumerable;

namespace FiveSpn.Logger.Server
{
    public class Service : BaseScript
    {
        private readonly LogMessageSeverity _severityPublic = LogMessageSeverity.Warning;
        private readonly LogMessageSeverity _severityPrivate = LogMessageSeverity.Debug;
        private int _daysLogsKept = 0;
        private DiscordWebhook _publicHook = new DiscordWebhook(API.GetResourceMetadata(API.GetCurrentResourceName(), "discord_webhook_public", 0));
        private DiscordWebhook _privateHook = new DiscordWebhook(API.GetResourceMetadata(API.GetCurrentResourceName(), "discord_webhook_private", 0));
        private readonly string _logDirectory = API.GetResourcePath(API.GetCurrentResourceName()) + "\\logs";
        private readonly string _logFile = "\\FiveSpnLog-" + DateTime.UtcNow.Date.ToString("MMddyyyy") + ".txt";
        private string LogPath => _logDirectory + _logFile;
        private readonly object _writeLock = new object();
        private readonly object _hookLock = new object();
        private const string ServerStartMessage = @"
#############################################################################################################
#      _________                     __________         __         .__       _______                        #
#     /   _____/ ____  __ _________  \______   \_____ _/  |_  ____ |  |__    \      \   ____   _____        #
#     \_____  \ /  _ \|  |  \_  __ \  |     ___/\__  \\   __\/ ___\|  |  \   /   |   \ /  _ \ /     \       #
#     /        (  <_> )  |  /|  | \/  |    |     / __ \|  | \  \___|   Y  \ /    |    (  <_> )  Y Y  \      #
#    /_______  /\____/|____/ |__|     |____|    (____  /__|  \___  >___|  / \____|__  /\____/|__|_|  /      #
#            \/                                      \/          \/     \/          \/             \/       # 
######################################## FiveSPN Services Initializing ######################################
# It looks like the server might have restarted?                                                            #
# If the logger resource was restarted directly all dependancies were likely stopped and must be restarted! #
# Visit itsthenom.com for more information about FiveSPN resources!                                         #
#############################################################################################################
";
        private const string ServerStartMessageDiscord = @"
```
###################################
#   __________________________    #
# /   _____/\______   \      \    #
# \_____  \  |     ___/   |   \   #
# /        \ |    |  /    |    \  #
#/_______  / |____|  \____|__  /  #
#        \/                  \/   # 
## FiveSPN Services Initializing ##
```
**It looks like the server might have restarted?**
*If the logger resource was restarted directly all dependancies were likely stopped and must be restarted!* 
Visit itsthenom.com for more information about FiveSPN resources!                                        
";

        public Service()
        {
            try
            {
                if (!Directory.Exists(_logDirectory))
                {
                    Directory.CreateDirectory(_logDirectory);
                }
                WriteStartupLines();
                Delay(1000);
            }
            catch (Exception e)
            {
                WriteMessageToConsole(new LogMessage(API.GetCurrentResourceName(), LogMessageSeverity.Critical,"Logging File Reset Error!\n"+e.Message));
            }
            if (int.TryParse(API.GetResourceMetadata(API.GetCurrentResourceName(), "log_level_public", 0), out var resultPub))_severityPublic = (LogMessageSeverity)resultPub;
            if (int.TryParse(API.GetResourceMetadata(API.GetCurrentResourceName(), "log_level_private", 0), out var resultPri))_severityPrivate = (LogMessageSeverity)resultPri;
            if (int.TryParse(API.GetResourceMetadata(API.GetCurrentResourceName(), "log_days", 0), out _daysLogsKept)) CleanLogs(_daysLogsKept);
            EventHandlers["FiveSPN-ServerLogToServer"] += new Action<string, int, string>(HandleServerLogMessage);
            EventHandlers["FiveSPN-ClientLogToServer"] += new Action<Player, string, int, string>(HandlePlayerLogMessage);
            ProcessServerLogMessage(new LogMessage(API.GetCurrentResourceName(), LogMessageSeverity.Info,"Server is starting or logger restarted!"));
            Delay(500);
            ProcessServerLogMessage(new LogMessage(API.GetCurrentResourceName(), LogMessageSeverity.Verbose,"Public log level set to " + _severityPublic + "."));
            ProcessServerLogMessage(new LogMessage(API.GetCurrentResourceName(), LogMessageSeverity.Verbose,"Private log level set to " + _severityPrivate + "."));
            ProcessServerLogMessage(new LogMessage(API.GetCurrentResourceName(), LogMessageSeverity.Verbose,"Logs are retained for " + _daysLogsKept + " days."));
        }

        private void WriteStartupLines()
        {
            try
            {
                lock (_writeLock)
                {
                    using (var writer = File.AppendText(LogPath))
                    {
                        writer.WriteLine(ServerStartMessage);
                        writer.Close();
                    }
                }
            }
            catch (Exception e)
            {
                WriteMessageToConsole(new LogMessage(API.GetCurrentResourceName(), LogMessageSeverity.Critical,"Startup Error!\n"+e.Message));
            }

            try
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(ServerStartMessage);
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            catch (Exception e)
            {
                WriteMessageToConsole(new LogMessage(API.GetCurrentResourceName(), LogMessageSeverity.Critical,"Startup Error!\n"+e.Message));
            }

            if (_publicHook._webhookUrl != "")
            {
                _publicHook.Send(ServerStartMessageDiscord);
            }
            
            if (_privateHook._webhookUrl != "")
            {
                _privateHook.Send(ServerStartMessageDiscord);
            }
        }

        private void CleanLogs(int days)
        {
            var dateValues = new List<string>();

            for (int i = 0; i < days; i++)
            {
                dateValues.Add(DateTime.UtcNow.AddDays(-i).Date.ToString("MMddyyyy"));
            }
            
            foreach (var fileName in Directory.EnumerateFiles(_logDirectory, "*.txt", SearchOption.TopDirectoryOnly))
            {
                var delete = true;
                foreach (var date in dateValues.Where(date => fileName.Contains(date)))
                {
                    delete = false;
                }

                if (!delete) continue;
                ProcessServerLogMessage(new LogMessage(API.GetCurrentResourceName(), LogMessageSeverity.Info,"Deleting out dated log file " + fileName));
                File.Delete(fileName);
            }
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
            if (logMessage.Severity <= _severityPublic && logMessage.Severity != LogMessageSeverity.Debug) WriteMessageToWebhook(logMessage, true);
            if (logMessage.Severity > _severityPrivate) return;
            WriteMessageToWebhook(logMessage, false);
            WriteMessageToLogFile(logMessage);
            WriteMessageToConsole(logMessage);
        }

        private Task WriteMessageToWebhook(LogMessage logMessage, bool isPublic)
        {
            lock (_hookLock)
            {
                Delay(1000);
                if (isPublic)
                {
                    if (_publicHook._webhookUrl != "")
                    {
                        _publicHook.Send($"[{logMessage.Source,20}] {logMessage.Message}");
                    }

                    return Task.FromResult("Sent");
                }

                if (_privateHook._webhookUrl != "")
                {
                    _privateHook.Send($"[{logMessage.Source,20}][{logMessage.Severity,8}] {DateTime.Now,-19} : {logMessage.Message}");
                }

                return Task.FromResult("Sent");
            }
        }

        private void WriteMessageToLogFile(LogMessage logMessage)
        {
            var messageCombined = $"[{logMessage.Source,20}][{logMessage.Severity,8}] {DateTime.Now,-19} : {logMessage.Message}";
            try
            {
                lock (_writeLock)
                {
                    using (var writer = File.AppendText(LogPath))
                    {
                        writer.WriteLine(messageCombined);
                        writer.Close();
                    }
                }
            }
            catch (Exception e)
            {
                WriteMessageToConsole(new LogMessage(API.GetCurrentResourceName(), LogMessageSeverity.Critical,"Logging File Error!\n"+e.Message));
            }
        }

        private void WriteMessageToConsole(LogMessage logMessage)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"Log>[{logMessage.Source,20}]");
                switch (logMessage.Severity)
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
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case LogMessageSeverity.Debug:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Console.Write($"[{logMessage.Severity,8}]{DateTime.UtcNow.ToString("HH:mm:ss"),-8} {logMessage.Message}\n");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception thrown while attempting to process log message to console.\n"+e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}