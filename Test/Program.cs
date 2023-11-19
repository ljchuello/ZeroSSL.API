using ZeroSSLApi;
using ZeroSSLApi.Client;
using ZeroSSLApi.Objets;
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
            string privateKey = zeroSslClient.Tools.GenerarClavePrivada();

            // Certificado
            Certificate certificate = await zeroSslClient.Certificate.Create(dominio, privateKey);
            string id = certificate.Id;

            Console.WriteLine($"{dominio} creado, valide y luego presione enter");
            Console.ReadLine();

            Download download = await zeroSslClient.Certificate.Download(certificate, privateKey);
            await File.WriteAllTextAsync("E:\\certificate.crt", download.CertificateCrt);
            await File.WriteAllTextAsync("E:\\private.key", download.CertificateCrt);

            Console.WriteLine("Enter para continuar");
            Console.ReadLine();
        }
    }
}