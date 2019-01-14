using Newtonsoft.Json;

namespace AREA_Back.Response
{
    public class PInterraction
    {
        [JsonProperty]
        public string Name;

        [JsonProperty]
        public string Description;
    }

    public class PService
    {
        [JsonProperty]
        public string Name;

        [JsonProperty]
        public PInterraction[] Actions;

        [JsonProperty]
        public PInterraction[] Reactions;
    }

    public class PClient
    {
        [JsonProperty]
        public string Host;
    }

    public class PServer
    {
        [JsonProperty]
        public int Current_time;

        [JsonProperty]
        public PService[] Services;
    }

    public class About
    {
        [JsonProperty]
        public PClient Client;

        [JsonProperty]
        public PServer Server;
    }
}
