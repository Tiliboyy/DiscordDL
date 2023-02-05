using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using GameStore;
using SimpleTCP;
using YamlDotNet.Serialization;

namespace DiscordDL;

public class Plugin : Plugin<Config>
{

    public override string Name { get; } = "DiscordDL";
    public override string Prefix { get; } = "DiscordDL";
    public override string Author { get; } = "Tiliboyy";
    public static Plugin Instance;
    public SimpleTcpServer Server = new();



    public override void OnEnabled()
    {
        Instance = this;
        Server.Start(IPAddress.Any, 12345);
        Server.DataReceived += Communication.DataReceived;

    }


    public override void OnDisabled()
    {
    }


}