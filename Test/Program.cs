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
            string dominio = $"202.entecprois.com";

            ZeroSslClient zeroSslClient = new ZeroSslClient(await File.ReadAllTextAsync("D:\\zerossltoken.txt"));

            // Llave privada
            AsymmetricCipherKeyPair asymmetricCipherKeyPair = zeroSslClient.Tools.GenerateRsaKeyPair();

            // Certificado
            Certificate certificate = await zeroSslClient.Certificate.Create(dominio, asymmetricCipherKeyPair);

            Console.WriteLine($"{dominio} creado, valide y luego presione enter");
            Console.ReadLine();

            Download download = await zeroSslClient.Certificate.Download(certificate);
            await File.WriteAllTextAsync("E:\\certificate.crt", download.CertificateCrt);
            await File.WriteAllTextAsync("E:\\private.key", zeroSslClient.Tools.GetPrivateKeyAsString(asymmetricCipherKeyPair.Private));
        }
    }
}