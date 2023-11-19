using Newtonsoft.Json;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using ZeroSSLApi.Objets.CsrCheck;
using System.IO;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace ZeroSSLApi.Client
{
    public class ToolsClient
    {
        private readonly string _token;

        public ToolsClient(string token)
        {
            _token = token;
        }

        public async Task<CsrCheck> CsrCheck(string csr)
        {
            // Preparing raw
            string raw = $"{{ \"csr\":\"{csr}\" }}";

            // Send post
            string jsonResponse = await Core.SendPostRequest($"/validation/csr?access_key={_token}", raw);

            // Return
            return JsonConvert.DeserializeObject<CsrCheck>(jsonResponse) ?? new CsrCheck();
        }

        public string GenerateCsr(AsymmetricCipherKeyPair keyPair, string commonName)
        {
            Pkcs10CertificationRequest pkcs10CertificationRequest = new Pkcs10CertificationRequest("SHA256WITHRSA", new X509Name($"CN={commonName}"), keyPair.Public, null, keyPair.Private);

            using (StringWriter stringWriter = new StringWriter())
            {
                PemWriter pemWriter = new PemWriter(stringWriter);
                pemWriter.WriteObject(pkcs10CertificationRequest);
                return stringWriter.ToString();
            }
        }


        public AsymmetricCipherKeyPair GenerateRsaKeyPair()
        {
            var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(), 2048);
            var generator = GeneratorUtilities.GetKeyPairGenerator("RSA");
            generator.Init(keyGenerationParameters);

            return generator.GenerateKeyPair();
        }

        public string GetPrivateKeyAsString(AsymmetricKeyParameter privateKey)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                PemWriter pemWriter = new PemWriter(stringWriter);
                pemWriter.WriteObject(privateKey);
                return stringWriter.ToString();
            }
        }
    }
}
