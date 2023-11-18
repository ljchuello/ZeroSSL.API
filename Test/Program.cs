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
            string dominio = $"{Guid.NewGuid()}.entecprois.com";

            ZeroSslClient zeroSslClient = new ZeroSslClient(await File.ReadAllTextAsync("D:\\zerossltoken.txt"));

            await zeroSslClient.Certificate.GetAsync("2d3add0243b8b17faf73fa176b60286a");

            
            Console.ReadLine();
        }
    }
}