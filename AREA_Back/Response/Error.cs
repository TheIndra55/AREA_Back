using Newtonsoft.Json;

namespace AREA_Back.Response
{
    public class Error
    {
        [JsonProperty]
        public int Code;

        [JsonProperty]
        public string Message;
    }
}
