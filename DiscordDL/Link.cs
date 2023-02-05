using Exiled.API.Extensions;
using Exiled.API.Features;
using Utils.NonAllocLINQ;
using YamlDotNet.Serialization;

namespace DiscordDL;

public class Link
{
    public static List<LinkedPlayer> LinkedPlayers = new();

    public static bool IsLinked(ulong steam64id)
    {
        var linkedlayers = LinkedPlayers.Where(x => x.Steam64Id == steam64id).ToList();
        if (linkedlayers.Count == 0)
        {
            return false;
        }
        var playerid = linkedlayers.Find(player => player.Steam64Id == steam64id);
        return playerid.Steam64Id != steam64id;
    }

    public ulong GetLinkedSteamID(ulong DiscordID)
    {
        var linkedlayers = LinkedPlayers.Where(x => x.UserID == DiscordID).ToList();
        return linkedlayers.Count == 0 ? (ulong)0 : linkedlayers.First().Steam64Id;
    }
    
    public static void Deletewarn(ulong DiscordID)
    {
        Read();
        var linkedlayers = LinkedPlayers.Where(x => x.UserID == DiscordID).ToList();
        foreach (var player in linkedlayers)
        {
            LinkedPlayers.Remove(player);
        }
        Update();
    }

    public static bool Linker(ulong UserID, int Code)
    {
        var e = LinkCommand.OpenLinks.Where(linkClass => linkClass.Code == Code).ToList();
        if (e.Count != 1) return false;
        var flag = AddLink(e.First().Steam64ID, UserID);
        return flag;

    }
    public static bool AddLink(ulong Steam64ID, ulong UserID)
    {

        Read();
        if (LinkedPlayers.Count(x => x.UserID == UserID) != 0) return false;
        LinkedPlayers.Add(new LinkedPlayer(){Steam64Id = Steam64ID, UserID = UserID});
        Update();
        return true;

    }
    public static void Update()
    {
        string yaml = new Serializer().Serialize(LinkedPlayers);
        File.WriteAllText(Path.Combine(Paths.Configs, "LinkedPlayers.yaml"), yaml);        
        string yamldata = File.ReadAllText(Path.Combine(Paths.Configs, "LinkedPlayers.yaml"));
        var e = new Deserializer().Deserialize<List<LinkedPlayer>>(new StringReader(yamldata));    
        LinkedPlayers = e;
    }
    public static void Read()
    {
        if (!File.Exists(Path.Combine(Paths.Configs, "LinkedPlayers.yaml")))
        {
            Update();
        }

        string yamldata = File.ReadAllText(Path.Combine(Paths.Configs, "LinkedPlayers.yaml"));
        var e = new Deserializer().Deserialize<List<LinkedPlayer>>(new StringReader(yamldata));
        LinkedPlayers = e;
        if (LinkedPlayers.Count == 0)
        {
            LinkedPlayers.Add(new LinkedPlayer() { Steam64Id = 0, UserID = 0 });
        }
    }
    public struct LinkedPlayer
    {
        public ulong UserID { get; set; }
        public ulong Steam64Id { get; set; }
    }

}