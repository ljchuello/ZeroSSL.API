using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
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
            string dominio = $"106.entecprois.com";

            ZeroSslClient zeroSslClient = new ZeroSslClient(await File.ReadAllTextAsync("D:\\zerossltoken.txt"));

            // Llave privada
            AsymmetricCipherKeyPair asymmetricCipherKeyPair = GenerateRsaKeyPair();
            string privateKey = GetPrivateKeyAsString(asymmetricCipherKeyPair.Private);

            // Certificado
            Certificate certificate = await zeroSslClient.Certificate.Create(dominio, asymmetricCipherKeyPair);
            string id = certificate.Id;

            Console.WriteLine($"{dominio} creado, valide y luego presione enter");
            Console.ReadLine();

            Download download = await zeroSslClient.Certificate.Download(certificate);
            await File.WriteAllTextAsync("E:\\certificate.crt", download.CertificateCrt);
            await File.WriteAllTextAsync("E:\\private.key", privateKey);

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
    }
}