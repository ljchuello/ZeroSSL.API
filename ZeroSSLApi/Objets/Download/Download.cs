using Newtonsoft.Json;

namespace ZeroSSLApi.Objets.Download
{
    public class Download
    {
        [JsonProperty("certificate.crt", NullValueHandling = NullValueHandling.Ignore)]
        public string CertificateCrt { get; set; } = string.Empty;

        [JsonProperty("ca_bundle.crt", NullValueHandling = NullValueHandling.Ignore)]
        public string CaBundleCrt { get; set; } = string.Empty;
    }
}
