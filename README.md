# Welcome to ZeroSSL.API

![CloudFlare.Dns](https://raw.githubusercontent.com/ljchuello/ZeroSSL.API/master/icon_128.png)

Library in .NET / C# that allows interacting with ZeroSSL APIs to manage certificates. Ideal for automating the creation and renewal of certificates without dealing with Let's Encrypt limits

**Note:** To use this library, you need to have the paid version of ZeroSSL.com. ZeroSSL doesn't provide API access in its free version

If you have any errors or questions, please create an [issue](https://github.com/ljchuello/ZeroSSL.API/issues), and I will respond as soon as possible

## Compatibility

This library is developed in .NET Standard 2.1 and is compatible with all .NET and .NET Core implementations (.NET Framework is not supported) , it can also be used in Console projects, Web API, Class Library and even with Blazor WASM.

| .NET implementation        	| Version support         	|
|----------------------------	|-------------------------	|
| .NET and .NET Core         	| 3.0, 3.1, 5.0, 6.0, 7.0 	|

## Installation

To install you must go to Nuget package manager and search for "ZeroSSL.API" and then install.

### [NuGet Package](https://www.nuget.org/packages/ZeroSSL.API)

    PM> Install-Package ZeroSSL.API
---
## Create certificate

```csharp
// First step is to instantiate the client and set the API key.
ZeroSslClient zeroSslClient = new ZeroSslClient("apiKey"));

// We create the AsymmetricCipherKeyPair object that will generate the private and public key
AsymmetricCipherKeyPair asymmetricCipherKeyPair = zeroSslClient.Tools.GenerateRsaKeyPair();

// Then we proceed to create the certificate. If everything is correct, it will return an object of type Certificate
Certificate certificate = await zeroSslClient.Certificate.Create(domain, asymmetricCipherKeyPair);

// Once the certificate is created, it's essential to store the private key used in its generation. This will be necessary later for publishing the certificate.
string privateKey = zeroSslClient.Tools.GetPrivateKeyAsString(asymmetricCipherKeyPair.Private);
```
---

## Verify Domain | File Challenge

Verifying the domain through `HTTP_CSR_HASH` is slightly more challenging, but its approval is almost instantaneous, making it well worth doing it this way

Once the certificate is created, it needs to be validated. Let's see an example of how to validate it using `HTTP_CSR_HASH`

To validate the domain correctly with the HTTP_CSR_HASH method, follow these steps in order:

1. Prepare the path where the .txt file will be stored.
1. Create the file and its content.
1. Resolve the challenge.

### 1. Prepare the path where the .txt file will be stored

It's the web path of the file or the web address to which ZeroSSL servers will make a web request to verify that the file exists and contains the verification information.

```csharp
// We can obtain the web address using the Certificate object
string route = certificate.Validation.OtherMethods.DomainDotCom.FileValidationUrlHttp;
```

In this case, `certificate.Validation.OtherMethods.DomainDotCom.FileValidationUrlHttp` will provide you with the exact path. It's up to you to implement the logic in your infrastructure or with the language of your choice to create the necessary route for validation

This is an example of the format for the URL `http://test.github.com/.well-known/pki-validation/613A9BBA456E7D714F39370CBAA5EC2A.txt`

### 2. Create the file and its content

Here I'll provide an example of how I create the file with its content. The logic should be similar for your case, regardless of the infrastructure you're using.

```csharp
// This is a real example of how the URL we need to prepare looks
// http://test.github.com/.well-known/pki-validation/EA8C215B907D0AB32AC5D08558AA0048.txt

// We can obtain the web address using the Certificate object
string route = certificate.Validation.OtherMethods.DomainDotCom.FileValidationUrlHttp;

// In this case, I'll use Uri to get the last part, which is the filename
string fileName = Path.GetFileName(new Uri(certificate.Validation.OtherMethods.DomainDotCom.FileValidationUrlHttp).AbsolutePath);

// This is the directory I have created and prepared for validation. Note that it logically aligns with step 1
string dir = $"C:\\inetpub\\wwwroot\\.well-known\\pki-validation\\{fileName}";

// On this line, I convert the array of file content to a string, and I add line breaks
string fileContent = string.Join("\n", certificate.Validation.OtherMethods.DomainDotCom.FileValidationContent);

// Proceeding to create the file
File.WriteAllText(dir, fileContent);
```

In this way, I create the .txt file. It's important to emphasize that the content of `certificate.Validation.OtherMethods.DomainDotCom.FileValidationContent` is an array, and we need to convert it to a string just like the example provided.

**Remember, you should apply your logic adapted to your infrastructure**

With this, we have completed this part.

### 3. Resolve the challenge

```csharp
// To resolve the challenge, pass the certificate as a parameter and specify the type of challenge.
bool verified = await zeroSslClient.Certificate.Challenge(certificate, ValidationMethod.HTTP_CSR_HASH);

// We can also pass the certificate ID instead of the complete certificate
bool verified = await zeroSslClient.Certificate.Challenge("certificateID", ValidationMethod.HTTP_CSR_HASH);
```

Once it returns True, wait for a couple of minutes, and you'll be able to download the certificate.

With this, we conclude the last step.

---
## Verify Domain | DNS Challenge

**NOTE**: When validating through DNS `CNAME_CSR_HASH`, the certificate stays in `pending_validation` status for a few minutes up to 12 hours. I strongly recommend using the `HTTP_CSR_HASH` validation method

Once the certificate is created, it needs to be validated. Let's see an example of how to validate it using `CNAME_CSR_HASH`.

| Record Type | Name | Value |
| :----------: | :----------: | :----------: |
| CNAME | certificate.Validation.OtherMethods.DomainDotCom.CnameValidationP1 | certificate.Validation.OtherMethods.DomainDotCom.CnameValidationP2 |

Here's a real example of how to verify the certificate for test.github.com using DNS validation

| Record Type | Name | Value |
| :----------: | :----------: | :----------: |
| CNAME | _1EA82E8C7EB83DB1A5861A02DDC8DE80.test.github.com | 3921D03C724EAA6EF42FCE5C8040A39E.464AC15B66374686070B777C674E46F7.51b54eb096d69ad.comodoca.com |

Once the DNS record is created in the domain, we proceed to resolve the challenge.

```csharp
// To resolve the challenge, pass the certificate as a parameter and specify the type of challenge.
bool verified = await zeroSslClient.Certificate.Challenge(certificate, ValidationMethod.CNAME_CSR_HASH);

// We can also pass the certificate ID instead of the complete certificate
bool verified = await zeroSslClient.Certificate.Challenge("certificateID", ValidationMethod.CNAME_CSR_HASH);
```

If `verified` returns True, it means the certificate is validated. If it returns False, an error has occurred, and you should check or correct any possible issues.

Once it returns True, wait a few minutes, and you can download the certificate

---
## Download Certificate

Once the certificate is in the `issued` state, you can download the certificate to implement it on `Nginx`, `IIS`, `Apache`, or the web server of your preference

To achieve this, you will need the certificate object you created and the private key string that was set earlier in the `Create Certificate` process

```csharp
// We download the files at this line
Download download = await zeroSslClient.Certificate.Download(certificate);

// You can also download by passing the certificate ID as a parameter
Download download = await zeroSslClient.Certificate.Download("df29fabbce961f1c997a4b9dcd8da0df");
```

Once the `zeroSslClient.Certificate.Download` function is executed, you will have the `certificate.crt` and `ca_bundle.crt` in the Download object. Additionally, in the first part, you set the `privateKey`. With this, you have all the information for the complete certificate, ready for implementation

As an extra, if you want to create the files to later copy and paste them into the web server, you can do it this way. In my case, for simplicity, I will place them on the D:\ drive

```csharp
// We write the private key file, whose variable we had set in the first step
File.WriteAllText("D:\\privateKey.key", privateKey);

// We write the certificate.crt file
File.WriteAllText("D:\\certificate.crt", certificateCrt);
```

In this way, we complete 100% of the process of creating a certificate for use in production
