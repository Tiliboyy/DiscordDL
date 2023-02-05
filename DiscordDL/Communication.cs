﻿using Exiled.API.Features;
using GameStore;
using SimpleTCP;
using YamlDotNet.Serialization;

namespace DiscordDL;

public static class Communication
{
    
    public static void DataReceived(object sender, Message message)
    {
        Log.Info("message.MessageString");
        var data = new Deserializer().Deserialize<CommunicationHelper.BotRequester>(
            message.MessageString.Remove(message.MessageString.Length - 1, 1));
        Log.Debug("Request Type: " + data.Type);
        switch (data.Type)
        {
            case CommunicationHelper.RequestType.PlayerList:

                var names = Player.List.Select(x => x.Nickname).ToList();
                message.ReplyList(names, CommunicationHelper.RequestType.PlayerList);
                break;
            case CommunicationHelper.RequestType.GameStoreMoney:
                
                string str = $"Du hast {GameStoreDatabase.Database.GetMoneyFromSteam64ID(data.Data)} DayLight Bits";
                message.ReplyString(str, CommunicationHelper.RequestType.GameStoreMoney);

                break;
            case CommunicationHelper.RequestType.Link:
                var e = new Deserializer().Deserialize<CommunicationHelper.Linker>(data.Data);
                Link.Linker(e.UserId, e.Code);
                break;
            default:
                Log.Error("Invalid request type recieved: " + data.Type);
                break;
        }

    }
    public static void ReplyString(this Message message, string data, CommunicationHelper.RequestType requestType)
    {
        var Data = new CommunicationHelper.PluginSender() { Data = data, Type = CommunicationHelper.DataType.Sring};
        var serializedData = new Serializer().Serialize(Data);
        message.ReplyLine(serializedData);
    }
    public static void ReplyList(this Message message, List<string> list, CommunicationHelper.RequestType requestType)
    {
        var Data = new CommunicationHelper.PluginSender() { Data = new Serializer().Serialize(list), Type = CommunicationHelper.DataType.List};
        var serializedData = new Serializer().Serialize(Data);
        message.ReplyLine(serializedData);
    }

}