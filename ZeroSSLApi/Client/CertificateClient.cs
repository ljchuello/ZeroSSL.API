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

        public async Task<Certificate> GetAsync(string certificateid)
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
    }
}
