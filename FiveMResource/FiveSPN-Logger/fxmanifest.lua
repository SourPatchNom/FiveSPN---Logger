fx_version 'bodacious'

name 'FiveSPN-Logger'
description 'Manages log messages for the server.'
author 'SourPatchNom'
version 'v1.0'
url 'https://itsthenom.com'

games { 'gta5' }

log_days '5' --How many days should logs be kept?
log_level_private '5' --What is the logging level for private logging? This information will be sent to the private_discord_webhook (if set), console, and txt log.
log_level_public '3' --What is the logging level for public logging? This information will be sent to the public_discord_webhook (if set). Public will never send DEBUG info!
discord_webhook_public ''
discord_webhook_private ''
--0 Critical
--1 Error
--2 Warning
--3 Info
--4 Verbose
--5 Debug

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