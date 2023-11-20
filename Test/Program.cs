using Org.BouncyCastle.Crypto;
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
            string domain = $"202.entecprois.com";

            ZeroSslClient zeroSslClient = new ZeroSslClient(await File.ReadAllTextAsync("D:\\zerossltoken.txt"));

            // We create the AsymmetricCipherKeyPair object that will generate the private and public key
            AsymmetricCipherKeyPair asymmetricCipherKeyPair = zeroSslClient.Tools.GenerateRsaKeyPair();

            // Then we proceed to create the certificate. If everything is correct, it will return an object of type Certificate
            Certificate certificate = await zeroSslClient.Certificate.Create(domain, asymmetricCipherKeyPair);

            Console.WriteLine($"{domain} creado, valide y luego presione enter");
            Console.ReadLine();

            Download download = await zeroSslClient.Certificate.Download(certificate);
            await File.WriteAllTextAsync("E:\\certificate.crt", download.CertificateCrt);
            await File.WriteAllTextAsync("E:\\private.key", zeroSslClient.Tools.GetPrivateKeyAsString(asymmetricCipherKeyPair.Private));
        }
    }
}