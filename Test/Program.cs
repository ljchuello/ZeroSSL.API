using Org.BouncyCastle.Crypto;
using System;
using ZeroSSLApi;
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
            ZeroSslClient zeroSslClient = new ZeroSslClient(await File.ReadAllTextAsync("D:\\zerossltoken.txt"));

            //// Domain to which the certificate will be added
            //string domain = $"ljchuellox2.entecprois.com";

            //// We create the AsymmetricCipherKeyPair object that will generate the private and public key
            //AsymmetricCipherKeyPair asymmetricCipherKeyPair = zeroSslClient.Tools.GenerateRsaKeyPair();

            //// Then we proceed to create the certificate. If everything is correct, it will return an object of type Certificate
            //Certificate certificate = await zeroSslClient.Certificate.Create(domain, asymmetricCipherKeyPair);

            //// Once the certificate is created, it's essential to store the private key used in its generation. This will be necessary later for publishing the certificate.
            //string privateKey = zeroSslClient.Tools.GetPrivateKeyAsString(asymmetricCipherKeyPair.Private);

            //// This is a real example of how the URL we need to prepare looks
            //// http://test.github.com/.well-known/pki-validation/EA8C215B907D0AB32AC5D08558AA0048.txt

            //// We can obtain the web address using the Certificate object
            //string route = certificate.Validation.OtherMethods.DomainDotCom.FileValidationUrlHttp;

            //// In this case, I'll use Uri to get the last part, which is the filename
            //string fileName = Path.GetFileName(new Uri(certificate.Validation.OtherMethods.DomainDotCom.FileValidationUrlHttp).AbsolutePath);

            //// This is the directory I have created and prepared for validation. Note that it logically aligns with step 1
            //string dir = $"C:\\inetpub\\wwwroot\\.well-known\\pki-validation\\{fileName}";

            //// On this line, I convert the array of file content to a string, and I add line breaks
            //string fileContent = string.Join("\n", certificate.Validation.OtherMethods.DomainDotCom.FileValidationContent);

            //// Proceeding to create the file
            //File.WriteAllText(dir, fileContent);

            //// To resolve the challenge, pass the certificate as a parameter and specify the type of challenge.
            //bool verified = await zeroSslClient.Certificate.Challenge(certificate, ValidationMethod.HTTP_CSR_HASH);

            //// To achieve this, you will need the certificate object you created and the private key string that was set earlier in the "Create Certificate" process

            //// We download the files at this line
            Download download = await zeroSslClient.Certificate.Download("bf3ab834855783285d2615fbd03b73a5");

            //// Setting the certificate.crt
            //string certificateCrt = download.CertificateCrt;

            //// We write the private key file, whose variable we had set in the first step
            //File.WriteAllText("D:\\privateKey.key", privateKey);

            //// We write the certificate.crt file
            //File.WriteAllText("D:\\certificate.crt", certificateCrt);
        }
    }
}