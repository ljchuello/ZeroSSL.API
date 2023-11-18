using System.Xml;
using ZeroSSLApi;
using ZeroSSLApi.Objets.Certificate;
using ZeroSSLApi.Objets.CsrCheck;

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
            string dominio = $"101.entecprois.com";

            ZeroSslClient zeroSslClient = new ZeroSslClient(await File.ReadAllTextAsync("D:\\zerossltoken.txt"));

            // Llave privada
            string privateKey = zeroSslClient.Tools.GenerarClavePrivada();

            // Certificado
            Certificate certificate = await zeroSslClient.Certificate.Create(dominio, privateKey);

            // Grabar fichero
            FileInfo fileInfo = new FileInfo(certificate.Validation.OtherMethods.DomainDotCom.FileValidationUrlHttp);
            string ficharo = $"{fileInfo}".Replace($"http://{dominio}", string.Empty);
            ficharo = ficharo.Replace("/", "\\");
            ficharo = $"C:\\inetpub\\wwwroot{ficharo}";
            await File.WriteAllTextAsync(ficharo, $"{string.Join("\n", certificate.Validation.OtherMethods.DomainDotCom.FileValidationContent)}");

            
            Console.ReadLine();
        }
    }
}