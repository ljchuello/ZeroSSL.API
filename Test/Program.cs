using ZeroSSLApi;
using ZeroSSLApi.Client;
using ZeroSSLApi.Objets;
using ZeroSSLApi.Objets.Certificate;

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
            for (int i = 1; i <= 15; i++)
            {
                string dominio = $"{Guid.NewGuid()}.entecprois.com";

                ZeroSslClient zeroSslClient = new ZeroSslClient(await File.ReadAllTextAsync("D:\\zerossltoken.txt"));

                // Llave privada
                string privateKey = zeroSslClient.Tools.GenerarClavePrivada();

                // Certificado
                Console.WriteLine($"Creando certificado {i}");
                Certificate certificate = await zeroSslClient.Certificate.Create(dominio, privateKey);
                string id = certificate.Id;

                // Grabar fichero
                Console.WriteLine($"Preparando validación {i}");
                FileInfo fileInfo = new FileInfo(certificate.Validation.OtherMethods.DomainDotCom.FileValidationUrlHttp);
                string ficharo = $"{fileInfo}".Replace($"http://{dominio}", string.Empty);
                ficharo = ficharo.Replace("/", "\\");
                ficharo = $"C:\\inetpub\\wwwroot{ficharo}";
                await File.WriteAllTextAsync(ficharo, $"{string.Join("\n", certificate.Validation.OtherMethods.DomainDotCom.FileValidationContent)}");

                Console.WriteLine($"Haciendo challenge {i}");
                bool challenge = await zeroSslClient.Certificate.Challenge(certificate, ValidationMethod.HTTP_CSR_HASH);
            }

            Console.WriteLine("Enter para continuar");
            Console.ReadLine();
        }
    }
}