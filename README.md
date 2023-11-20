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
string domain = $"text.deployrise.com";

// We create the AsymmetricCipherKeyPair object that will generate the private and public key
AsymmetricCipherKeyPair asymmetricCipherKeyPair = zeroSslClient.Tools.GenerateRsaKeyPair();

// Then we proceed to create the certificate. If everything is correct, it will return an object of type Certificate
Certificate certificate = await zeroSslClient.Certificate.Create(domain, asymmetricCipherKeyPair);
```
