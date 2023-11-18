using ZeroSSLApi.Client;

namespace ZeroSSLApi
{
    public class ZeroSslClient
    {
        public string Token { get; private set; }

        public ZeroSslClient(string token)
        {
            Token = token;
            Certificate = new CertificateClient(token);
            Tools = new ToolsClient(token);
        }

        public CertificateClient Certificate { get; private set; }
        public ToolsClient Tools { get; private set; }
    }
}
