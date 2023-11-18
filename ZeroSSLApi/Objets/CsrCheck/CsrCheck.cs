using Newtonsoft.Json;

namespace ZeroSSLApi.Objets.CsrCheck
{
    public class CsrCheck
    {
        [JsonProperty("valid", NullValueHandling = NullValueHandling.Ignore)]
        public bool Valid { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public object Error { get; set; }
    }
}
