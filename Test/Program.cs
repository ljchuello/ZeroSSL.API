using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using ZeroSSLApi;
using ZeroSSLApi.Objets.Certificate;
using ZeroSSLApi.Objets.Download;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            string dominio = $"app.entecprois.com";

            ZeroSslClient zeroSslClient = new ZeroSslClient(await File.ReadAllTextAsync("D:\\zerossltoken.txt"));

            // Llave privada
            //string privateKey = zeroSslClient.Tools.GenerarClavePrivada();
            AsymmetricCipherKeyPair keyPair = GenerateRsaKeyPair();
            string privateKey = GetPrivateKeyAsString(keyPair.Private);

            string csr = GenerateCsr(keyPair, dominio);
            csr = csr.Trim();
            csr = csr.Replace("\n", string.Empty);
            csr = csr.Replace("\r", string.Empty);

            var check = await zeroSslClient.Tools.CsrCheck(csr);

            // Certificado
            Certificate certificate = await zeroSslClient.Certificate.Create(dominio, csr);
            string id = certificate.Id;

            Console.WriteLine($"{dominio} creado, valide y luego presione enter");
            Console.ReadLine();

            Download download = await zeroSslClient.Certificate.Download(certificate);
            await File.WriteAllTextAsync("E:\\certificate.crt", download.CertificateCrt);
            await File.WriteAllTextAsync("E:\\private.key", privateKey);

            //// Paso 1: Generar la Llave Privada
            //AsymmetricCipherKeyPair keyPair = GenerateRsaKeyPair();
            //string privateKeyString = GetPrivateKeyAsString(keyPair.Private);
            //Console.WriteLine("Llave privada:");
            //Console.WriteLine(privateKeyString);

            //// Paso 2: Generar el CSR
            //X509Certificate csr = GenerateCsr(keyPair, dominio);
            //SaveCsr(csr, "tuDominio.csr");

            Console.WriteLine("Enter para continuar");
            Console.ReadLine();
        }

        static AsymmetricCipherKeyPair GenerateRsaKeyPair()
        {
            var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(), 2048);
            var generator = GeneratorUtilities.GetKeyPairGenerator("RSA");
            generator.Init(keyGenerationParameters);

            return generator.GenerateKeyPair();
        }

        static string GetPrivateKeyAsString(AsymmetricKeyParameter privateKey)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                PemWriter pemWriter = new PemWriter(stringWriter);
                pemWriter.WriteObject(privateKey);
                return stringWriter.ToString();
            }
        }

        static string GenerateCsr(AsymmetricCipherKeyPair keyPair, string commonName)
        {
            var csrGenerator = new Org.BouncyCastle.Pkcs.Pkcs10CertificationRequest("SHA256WITHRSA", new X509Name($"CN={commonName}"), keyPair.Public, null, keyPair.Private);

            using (StringWriter stringWriter = new StringWriter())
            {
                PemWriter pemWriter = new PemWriter(stringWriter);
                pemWriter.WriteObject(csrGenerator);
                return stringWriter.ToString();
            }
        }
    }
}