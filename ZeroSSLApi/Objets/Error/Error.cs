using Newtonsoft.Json;

namespace ZeroSSLApi.Objets.Error
{
    public class Error
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public long Code { get; set; } = 0;

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; } = string.Empty;
    }
}