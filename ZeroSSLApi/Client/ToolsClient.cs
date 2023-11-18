using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ZeroSSLApi.Objets.CsrCheck;

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
            string raw = $"{{ \"csr\":\"{csr.Trim()}\" }}";

            // Send post
            string jsonResponse = await Core.SendPostRequest($"/validation/csr?access_key={_token}", raw);

            // Return
            return JsonConvert.DeserializeObject<CsrCheck>(jsonResponse) ?? new CsrCheck();
        }

        public string GenerarClavePrivada(int longitudClave = 2048)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.KeySize = longitudClave;
                string clavePrivada = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

                return clavePrivada.Trim();
            }
        }

        public string KeyToPem(string clavePrivadaBase64)
        {
            // Agrega las etiquetas PEM necesarias
            string pemFormat = $"-----BEGIN PRIVATE KEY-----\n{clavePrivadaBase64}\n-----END PRIVATE KEY-----";
            return pemFormat.Trim();
        }

        public string GenerarCSR(string dominio, string clavePrivadaBase64)
        {
            byte[] clavePrivadaBytes = Convert.FromBase64String(clavePrivadaBase64);

            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(clavePrivadaBytes, out _);

                var request = new CertificateRequest(
                    new X500DistinguishedName($"CN={dominio}"),
                    rsa,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1
                );

                var csrBytes = request.CreateSigningRequest();
                return Convert.ToBase64String(csrBytes).Trim();
            }
        }
    }
}
