# Welcome to ZeroSSL.API

![CloudFlare.Dns](https://raw.githubusercontent.com/ljchuello/ZeroSSL.API/master/icon_128.png)

Library in .NET / C# that allows interacting with ZeroSSL APIs to manage certificates. Ideal for automating the creation and renewal of certificates without dealing with Let's Encrypt limits

## Compatibility

This library is developed in .NET Standard 2.1 and is compatible with all .NET and .NET Core implementations (.NET Framework is not supported) , it can also be used in Console projects, Web API, Class Library and even with Blazor WASM.

| .NET implementation        	| Version support         	|
|----------------------------	|-------------------------	|
| .NET and .NET Core         	| 3.0, 3.1, 5.0, 6.0, 7.0 	|

## Installation

To install you must go to Nuget package manager and search for "ZeroSSL.API" and then install.

### [NuGet Package](https://www.nuget.org/packages/ZeroSSL.API)

    PM> Install-Package ZeroSSL.API

## Create certificate

```csharp
// First step is to instantiate the client and set the API key.
ZeroSslClient zeroSslClient = new ZeroSslClient("apiKey"));

// Domain to which the certificate will be added
string domain = "text.deployrise.com";

// We create the AsymmetricCipherKeyPair object that will generate the private and public key
AsymmetricCipherKeyPair asymmetricCipherKeyPair = zeroSslClient.Tools.GenerateRsaKeyPair();

// Then we proceed to create the certificate. If everything is correct, it will return an object of type Certificate
Certificate certificate = await zeroSslClient.Certificate.Create(domain, asymmetricCipherKeyPair);
```

## Verify Domains | DNS Challenge

Once the certificate is created, it needs to be validated. Let's see an example of how to validate it using DNS.

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

If "verified" returns True, it means the certificate is validated. If it returns False, an error has occurred, and you should check or correct any possible issues.
