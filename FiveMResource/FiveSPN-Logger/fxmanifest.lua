fx_version 'bodacious'

name 'FiveSPN-Logger'
description 'Manages log messages for the server.'
author 'SourPatchNom'
version 'v1.0'
url 'https://itsthenom.com'

games { 'gta5' }

log_level '5'

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
	"FiveSpn.Logger.Library.dll"
}