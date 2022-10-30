fx_version 'bodacious'

name 'FiveSPN-Logger'
description 'Manages log messages for the server.'
author 'SourPatchNom'
version 'v1.0'
url 'https://itsthenom.com'

games { 'gta5' }

server_scripts {
	"FiveSpn.Logger.Server.net.dll",
}

client_scripts {
	"FiveSpn.Logger.Client.net.dll",
}

files { 
	"FiveSpn.Logger.Library.dll"
}