using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ZeroSSLApi.Objets.Certificate;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using ZeroSSLApi.Objets;
using ZeroSSLApi.Objets.Download;

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
        /// <param name="privateKey">Private key with which the certificate will be generated</param>
        /// <returns></returns>
        public async Task<Certificate> Create(string domain, string privateKey)
        {
            // Csr
            ToolsClient toolsClient = new ToolsClient(_token);
            string csr = toolsClient.GenerarCSR(domain, privateKey);

            // Set
            string raw = $"{{ \"certificate_domains\": \"{domain}\", \"certificate_csr\": \"{csr.Trim()}\" }}";

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

        public async Task<bool> Challenge(string certificateId, ValidationMethod validationMethod)
        {
           // Set
            string raw = $"{{ \"validation_method\": \"{validationMethod}\" }}";

            // Send
            string json = await Core.SendPostRequest($"/certificates/{certificateId}/challenges?access_key={_token}", raw);

            // To object
            Certificate certificate = JsonConvert.DeserializeObject<Certificate>(json);

            // Get DomainDotCom
            JObject result = JObject.Parse(json);

            if (string.IsNullOrWhiteSpace(certificate.Id) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Challenge(Certificate certificate, ValidationMethod validationMethod)
        {
            return await Challenge(certificate.Id, validationMethod);
        }

        public async Task<Download> Download(string certificateId, string privateKey)
        {
            // Send
            string json = await Core.SendGetRequest($"/certificates/{certificateId}/download/return?access_key={privateKey}");

            // To object
            Download download = JsonConvert.DeserializeObject<Download>(json) ?? new Download();

            // Set private key
            download.PrivateKey = $"-----BEGIN PRIVATE KEY-----\n{privateKey}\n-----END PRIVATE KEY-----";

            // Freedom
            return download;
        }

        public async Task<Download> Download(Certificate certificate, string privateKey)
        {
            // Send
            string json = await Core.SendGetRequest($"/certificates/{certificate.Id}/download/return?access_key={_token}");

            // To object
            Download download = JsonConvert.DeserializeObject<Download>(json) ?? new Download();

            const int lineLength = 64;

            // Private key
            string formattedKey = $"-----BEGIN PRIVATE KEY-----\n";
            for (int i = 0; i < privateKey.Length; i += lineLength)
            {
                int remainingLength = Math.Min(lineLength, privateKey.Length - i);
                formattedKey += privateKey.Substring(i, remainingLength) + "\n";
            }
            formattedKey += $"-----END PRIVATE KEY-----";

            // Set private key
            download.PrivateKey = formattedKey;

            // Freedom
            return download;
        }
    }
}
