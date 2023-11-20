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

        /// <summary>
        /// Allows validating whether the CSR is valid or not
        /// </summary>
        /// <param name="csr"></param>
        /// <returns></returns>
        public async Task<CsrCheck> CsrCheck(string csr)
        {
            // Preparing raw
            string raw = $"{{ \"csr\":\"{csr}\" }}";

            // Send post
            string jsonResponse = await Core.SendPostRequest($"/validation/csr?access_key={_token}", raw);

            // Return
            return JsonConvert.DeserializeObject<CsrCheck>(jsonResponse) ?? new CsrCheck();
        }

        /// <summary>
        /// Generates the CSR
        /// </summary>
        /// <param name="keyPair"></param>
        /// <param name="commonName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Generates the AsymmetricCipherKeyPair object that will temporarily store the private key and public key
        /// </summary>
        /// <returns></returns>
        public AsymmetricCipherKeyPair GenerateRsaKeyPair()
        {
            var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(), 2048);
            var generator = GeneratorUtilities.GetKeyPairGenerator("RSA");
            generator.Init(keyGenerationParameters);

            return generator.GenerateKeyPair();
        }

        /// <summary>
        /// Returns the private key using the AsymmetricKeyParameter object
        /// </summary>
        /// <param name="asymmetricKeyParameter"></param>
        /// <returns></returns>
        public string GetPrivateKeyAsString(AsymmetricKeyParameter asymmetricKeyParameter)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                PemWriter pemWriter = new PemWriter(stringWriter);
                pemWriter.WriteObject(asymmetricKeyParameter);
                return stringWriter.ToString();
            }
        }
    }
}
