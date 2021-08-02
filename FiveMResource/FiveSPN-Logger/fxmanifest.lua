fx_version 'bodacious'

name 'FiveSPN-Logger'
description 'Manages log messages for the server.'
author 'SourPatchNom'
version 'v1.0'
url 'https://itsthenom.com'

utc_offset = '0' -- The offset in hours from UTC time for the server
run_speed_modifier = '1.1999' -- Run speed is multiplied times this number as float x.xxx
discord_rp_key = '700338921246294026'
discord_rp_asset = 'sp512'
discord_rp_asset_small = 'sp512'

games { 'gta5' }

server_script {
	"FiveSpnLoggerClientToServer.net.dll",
}

files { 
	"FiveSpnLoggerServerLibrary.dll",
	"FiveSpnLoggerClientLibrary.dll",
}