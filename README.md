# FiveSPN---Logger

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
[Join the project discord at itsthenom.com](http://itsthenom.com/)

## License

In the hopes that the greater community may benefit, you may use this code under the [GNU Affero General Public License v3.0](LICENSE).

This resource distribution utilizes the [Newtonsoft.JSON Library](https://github.com/JamesNK/Newtonsoft.Json) under the [MIT License](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md).

## Credits
* <b>Sloosecannon</b> for inspiration and rubber ducky assistance during the initial conception of all this in 2020.
* <b>AGHMatti</b> I think... for reference on the http helper, really wish I could locate the source repo now.
