# FiveSPN---Logger

![FiveSPN-Logger Banner](https://cdn.discordapp.com/attachments/871610618950615052/1039346989260877885/fspnlogger.png)

FiveSPN-Logger is a cross resource FiveM logging service that permits txt file, console, and discord logging of anything you send to it. Events are available for both server and client sources. Discord logs can be sent to a public webhook for things like "server starting" and a private webhook for things like errors or events (IE someone did/spawned/deleted this or this resource had an error etc).

## Installation

Copy the folder inside of the FiveMResource folder into your server's resources folder and add it to the server configuration file. 

### In server config
```
ensure FiveSPN-Logger
```

A complete server use package with all FiveSPN resources is available [here](https://github.com/SourPatchNom/FiveSPN---Suite)

## Usage

### In Server Resource

- ***FiveSPN-LogToDiscord*** -- *(bool public, string source, string message)* -- Use in server resources to send public or private updates in discord.
- ***FiveSPN-LogToServer*** -- *(string source, int level, string message)* -- Use in server resources to add a log entry.

```c#
TriggerEvent("FiveSPN-LogToDiscord", true, API.GetCurrentResourceName(), "Initializing");
TriggerEvent("FiveSPN-LogToServer", API.GetCurrentResourceName(), 3, "Initializing");
```

### In Client Resource
- ***FiveSPN-LogFromClient*** -- *(string source, int level, string message)* -- Used in client resources to add a log entry on the server from a client.
- ***FiveSPN-LogToClient*** -- *(string source, int level, string message)* -- Used in client resources to add a log entry on the client console.

```c#
TriggerEvent("FiveSPN-LogToClient", API.GetCurrentResourceName(), 3, "Initializing");
TriggerEvent("FiveSPN-LogFromClient", API.GetCurrentResourceName(), 3, "Initializing");
```

## Reference

### Log Levels
1. Critical
2. Error
3. Warning
4. Info
5. Verbose
6. Debug

*Discord will never get 6 - DEBUG level info.*

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Discord
FiveM development can be even more amazing if we work together to grow the open source community! Lets Collab! Join the project discord at [itsthenom.com!](http://itsthenom.com/)

## Licenses

In the hopes that the greater community may benefit, you may use this code under the [GNU Affero General Public License v3.0](LICENSE).

This resource distribution utilizes the [Newtonsoft.JSON Library](https://github.com/JamesNK/Newtonsoft.Json) under the [MIT License](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md).

This software references the CitizenFX.Core.Server and CitizenFX.Core.Client nuget packages (c) 2017-2020 the CitizenFX Collective used under [license](https://github.com/citizenfx/fivem/blob/master/code/LICENSE) and under the [FiveM Service Agreement](https://fivem.net/terms)

Never heard of FiveM? Learn more about the CitizenFX FiveM project [here](https://fivem.net/)

## Credits
* <b>Sloosecannon</b> for inspiration and rubber ducky assistance during the initial conception of all this in 2020.
* <b>AGHMatti</b> I think... for reference on the http helper, really wish I could locate the source repo now.
