
public class CommunicationHelper
{
    public enum RequestType
    {
        PlayerList,
        GameStoreMoney,
    }
    public struct ListData
    {
        public string SentStringData { get; set; }
        public List<string> ReplyData { get; set; }

    }
    public struct OutputList
    {
        public RequestType Type { get; set; }
        public string Data { get; set; }
    }
    public struct StringData
    {
        public string ReturnData { get; set; }
        public string SentData { get; set; }

    }

    public struct SentData
    {
        public RequestType Type { get; set; }
        public StringData StringData { get; set; }
        public ListData ListData { get; set; }
    }
}