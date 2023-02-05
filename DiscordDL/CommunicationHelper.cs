
public class CommunicationHelper
{
    public enum RequestType
    {
        PlayerList,
        GameStoreMoney,
        Link,
    }
    public enum DataType
    {
        List,
        Sring,
        Link,
    }
    public struct BotRequester
    {
        public RequestType Type { get; set; }
        public string Data { get; set; }
    }
    public struct PluginSender
    {
        public DataType Type { get; set; }
        public string Data { get; set; }
    }
    public struct Linker
    {
        public ulong UserId { get; set; }
        public int Code { get; set; }
        public bool Linked { get; set; }
    }
}