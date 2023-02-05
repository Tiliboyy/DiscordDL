using Exiled.API.Features;
using GameStore;
using SimpleTCP;
using YamlDotNet.Serialization;

namespace DiscordDL;

public static class Communication
{
    public static void ReplyString(this Message message, string data, CommunicationHelper.RequestType requestType)
    {
        var Data = new CommunicationHelper.StringData() { SentData = new Serializer().Serialize(data) };
        var serializedData = new Serializer().Serialize(Data);
        message.ReplyLine(serializedData);
    }
    public static void ReplyList(this Message message, List<string> list, CommunicationHelper.RequestType requestType)
    {
        var Data = new CommunicationHelper.ListData() { ReplyData = list };
        var serializedData = new Serializer().Serialize(Data);
        message.ReplyLine(serializedData);
    }


    public static void DataReceived(object sender, Message message)
    {
        Log.Info("recieved Request!");
        var data = new Deserializer().Deserialize<CommunicationHelper.SentData>(
            message.MessageString.Remove(message.MessageString.Length - 1, 1));
        Log.Debug("Request Type: " + data.Type);
        switch (data.Type)
        {
            case CommunicationHelper.RequestType.PlayerList:

                var names = Player.List.Select(x => x.Nickname).ToList();
                message.ReplyList(names, CommunicationHelper.RequestType.PlayerList);
                break;
            case CommunicationHelper.RequestType.GameStoreMoney:
                
                var ply = Player.Get(data.StringData.SentData);
                string str = $"Du hast {ply.GetMoney()} DayLight Bits";
                message.ReplyString(str, CommunicationHelper.RequestType.GameStoreMoney);

                break;
            default:
                Log.Error("Invalid request type recieved: " + data.Type);
                break;
        }

    }

}