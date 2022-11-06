fx_version 'bodacious'

name 'FiveSPN-Logger'
description 'Manages log messages for the server.'
author 'SourPatchNom'
version 'v1.0'
url 'https://itsthenom.com'

games { 'gta5' }

log_days '5' --How many days should logs be kept?
log_level_server '5' --What is the logging level for the server?
log_to_discord 'true'
log_level_private '3' --What is the logging level for private discord logging? This information will be sent to the private_discord_webhook (if set), console, and txt log.
log_level_public '2' --What is the logging level for public logging? This information will be sent to the public_discord_webhook (if set). 
discord_webhook_public ''
discord_webhook_private ''

server_scripts {
	"FiveSpn.Logger.Server.net.dll",
}

client_scripts {
	"FiveSpn.Logger.Client.net.dll",
}

files { 
	"FiveSpn.Logger.Library.dll",
	"Newtonsoft.Json.dll"
}

--Events handled:
--FiveSPN-LogToDiscord  - bool public, string source, string message -- Use to send public updates about the server.
--FiveSPN-LogToServer - string source, int level, string message -- Use to add a log entry from another server resource.
--FiveSPN-LogFromClient - string source, int level, string message -- Use to add a log entry on the server from a client.

--Log Levels
--0 Critical
--1 Error
--2 Warning
--3 Info
--4 Verbose
--5 Debug

--***NOTE*** Discord will never get DEBUG info!