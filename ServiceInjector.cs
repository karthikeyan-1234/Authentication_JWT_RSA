using System.Security.Cryptography;
using System.Text;

namespace Authentication_JWT_RSA
{
    public static class ServiceInjector
    {
        public static IServiceCollection AddRsaKeys(this IServiceCollection services, SecurityOptions options)
        {
            string? keysFolder = Path.GetDirectoryName(options.PrivateKeyFilePath);

            if(!Directory.Exists(keysFolder) && keysFolder is not null) Directory.CreateDirectory(keysFolder);

            var rsa = RSA.Create();
            string privateKeyXml = rsa.ToXmlString(true);
            string publicKeyXml = rsa.ToXmlString(false);

            using var privateFile = File.Create(options.PrivateKeyFilePath);
            using var publicKeyFile = File.Create(options.PublicKeyFilePath);

            privateFile.Write(Encoding.UTF8.GetBytes(privateKeyXml));
            publicKeyFile.Write(Encoding.UTF8.GetBytes(publicKeyXml));

            return services;
        }
    }
}
