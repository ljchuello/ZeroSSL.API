using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ZeroSSLApi.Objets.Certificate;
using System.Threading.Tasks;
using ZeroSSLApi.Objets;
using ZeroSSLApi.Objets.Download;
using Org.BouncyCastle.Crypto;

namespace ZeroSSLApi.Client
{
    public class CertificateClient
    {
        private readonly string _token;

        public CertificateClient(string token)
        {
            _token = token;
        }

        /// <summary>
        /// Gets a certificate based on the certificate ID
        /// </summary>
        /// <param name="certificateId">Certificate ID</param>
        /// <returns></returns>
        public async Task<Certificate> Get(string certificateId)
        {
            // Get list
            string json = await Core.SendGetRequest($"/certificates/{certificateId}?access_key={_token}");

            // To object
            Certificate certificate = JsonConvert.DeserializeObject<Certificate>(json);

            // Get DomainDotCom
            JObject result = JObject.Parse(json);
            certificate.Validation.OtherMethods.DomainDotCom = JsonConvert.DeserializeObject<DomainDotCom>($"{result["validation"]["other_methods"][certificate.CommonName]}") ?? new DomainDotCom();

            // Return
            return certificate;
        }

        /// <summary>
        /// 
        /// It allows creating a certificate, and it's returned as an object. After creating the certificate, you'll need to verify it.
        /// </summary>
        /// <param name="domain">Domain for which to generate the certificate</param>
        /// <param name="asymmetricCipherKeyPair"></param>
        /// <returns></returns>
        public async Task<Certificate> Create(string domain, AsymmetricCipherKeyPair asymmetricCipherKeyPair)
        {
            ToolsClient toolsClient = new ToolsClient(_token);

            // CSR
            string csr = toolsClient.GenerateCsr(asymmetricCipherKeyPair, domain);
            csr = csr.Trim();
            csr = csr.Replace("\r", string.Empty);
            csr = csr.Replace("\n", string.Empty);

            // Set
            string raw = $"{{ \"certificate_domains\": \"{domain}\", \"certificate_csr\": \"{csr}\" }}";

            // Send
            string json = await Core.SendPostRequest($"/certificates?access_key={_token}", raw);

            // To object
            Certificate certificate = JsonConvert.DeserializeObject<Certificate>(json);

            // Get DomainDotCom
            JObject result = JObject.Parse(json);
            certificate.Validation.OtherMethods.DomainDotCom = JsonConvert.DeserializeObject<DomainDotCom>($"{result["validation"]["other_methods"][certificate.CommonName]}") ?? new DomainDotCom();

            // Return
            return certificate;
        }

        /// <summary>
        /// Solve the challenge to validate the certificate
        /// </summary>
        /// <param name="certificateId"></param>
        /// <param name="validationMethod"></param>
        /// <returns></returns>
        public async Task<bool> Challenge(string certificateId, ValidationMethod validationMethod)
        {
           // Set
            string raw = $"{{ \"validation_method\": \"{validationMethod}\" }}";

            // Send
            string json = await Core.SendPostRequest($"/certificates/{certificateId}/challenges?access_key={_token}", raw);

            // To object
            Certificate certificate = JsonConvert.DeserializeObject<Certificate>(json);

            if (string.IsNullOrWhiteSpace(certificate.Id) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Solve the challenge to validate the certificate
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="validationMethod"></param>
        /// <returns></returns>
        public async Task<bool> Challenge(Certificate certificate, ValidationMethod validationMethod)
        {
            return await Challenge(certificate.Id, validationMethod);
        }

        /// <summary>
        /// Download the certificate
        /// </summary>
        /// <param name="certificateId"></param>
        /// <returns></returns>
        public async Task<Download> Download(string certificateId)
        {
            // Send
            string json = await Core.SendGetRequest($"/certificates/{certificateId}/download/return?access_key={_token}");

            // To object
            Download download = JsonConvert.DeserializeObject<Download>(json) ?? new Download();

            // Freedom
            return download;
        }

        /// <summary>
        /// Download the certificate
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public async Task<Download> Download(Certificate certificate)
        {
            return await Download(certificate.Id);
        }
    }
}
