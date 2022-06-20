using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace SecureResource.Service.Movie.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static void ConfigureProductionKeyVault(this IConfigurationBuilder config)
        {
            var builtConfig = config.Build();
            var store = new X509Store(StoreLocation.CurrentUser);
            
            store.Open(OpenFlags.ReadOnly);
            var certs = store.Certificates
                .Find(X509FindType.FindByThumbprint,
                        builtConfig["KeyVaultConfig:CertificateThumb"], false);

            config.AddAzureKeyVault(
                    builtConfig["KeyVaultConfig:KVURL"],
                    builtConfig["KeyVaultConfig:ClientId"],
                    certs.OfType<X509Certificate2>().Single());

            store.Close();
        }
        public static void ConfigureDevelopmentKeyVault(this IConfigurationBuilder config)
        {
            var builtConfig = config.Build();

            string kvURL = builtConfig["KeyVaultConfig:KVURL"];
            string tenantId = builtConfig["KeyVaultConfig:TenantId"];
            string clientId = builtConfig["KeyVaultConfig:ClientId"];
            string clientSecret = builtConfig["KeyVaultConfig:ClientSecret"];

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            var client = new SecretClient(new Uri(kvURL), credential);
            config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
        }
    }
}
