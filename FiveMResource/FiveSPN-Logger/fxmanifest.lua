fx_version 'bodacious'

name 'FiveSPN-Logger'
description 'Manages log messages for the server.'
author 'SourPatchNom'
version 'v1.0'
url 'https://itsthenom.com'

games { 'gta5' }

server_script {
	"FiveSpnLoggerClientToServer.net.dll",
}

files { 
	"FiveSpnLoggerServerLibrary.dll",
	"FiveSpnLoggerClientLibrary.dll",
}