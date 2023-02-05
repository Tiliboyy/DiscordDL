using System;
using System.Linq;
using System.Security.Cryptography;
using CommandSystem;
using Discord;
using DiscordDL;
using MEC;
using PluginAPI.Core;
using Random = UnityEngine.Random;


[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal class LinkCommand : ICommand
{
    public string Command { get; } = "Link";

    public string[] Aliases { get; } = Array.Empty<string>();

    public string Description { get; } = "Links a Player";

    public static List<LinkClass> OpenLinks = new();

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Exiled.API.Features.Player.Get(sender);
        if (ulong.TryParse(player.RawUserId, out var iResult))
        {
            if (Link.IsLinked(iResult))
            {
                response = $"Du bist bereits verlinkt!";
                return true;
            }
            response = GenerateLinkCode(iResult);
        }
                
        response = $"response";
        return true;
    }
    public static string GenerateLinkCode(ulong steam64ID)
    {
        var Code = GenerateCode();
        OpenLinks.Add(new LinkClass(steam64ID, 60, Code));
        return "Dein Code ist " + Code + " er ist für 60 Sekunden aktiv!";

    }

    public static int GenerateCode()
    {
        var f = Random.Range(1000, 9999);
        return f;
    }
    public static IEnumerator<float> Timer()
    {
        for ( ; ;)
        {
            yield return Timing.WaitForSeconds(1f);
            foreach (var pair in OpenLinks)
            {
                pair.Time -= 1;
                if (pair.Time == 0) OpenLinks.Remove(pair);
            }
        } 
    }
    public class LinkClass
    {
        public LinkClass(ulong steam64ID, float time, int code)
        {
            Steam64ID = steam64ID;
            Time = time;
            Code = code;
        }

        public ulong Steam64ID { get; set; }

        public int Code { get; set; }
        public float Time { get; set; }
    }

}