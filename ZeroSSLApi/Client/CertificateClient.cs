using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ZeroSSLApi.Objets.Certificate;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ZeroSSLApi.Client
{
    public class CertificateClient
    {
        private readonly string _token;

        public CertificateClient(string token)
        {
            _token = token;
        }

        public async Task<Certificate> Get(string certificateid)
        {
            // Get list
            string json = await Core.SendGetRequest($"/certificates/{certificateid}?access_key={_token}");

            // To object
            Certificate certificate = JsonConvert.DeserializeObject<Certificate>(json);

            // Get DomainDotCom
            JObject result = JObject.Parse(json);
            certificate.Validation.OtherMethods.DomainDotCom = JsonConvert.DeserializeObject<DomainDotCom>($"{result["validation"]["other_methods"][certificate.CommonName]}") ?? new DomainDotCom();

            // Return
            return certificate;
        }

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
    }
}
