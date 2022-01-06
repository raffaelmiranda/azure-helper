using Azure.Helper.KeyVault;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args, IConfiguration configuration)
        {
            //TODO: Implement Interface and read appsettings
            Secret keyVault = new Secret(
                configuration["AzureKeyVault:KeyVaultName"],
                configuration["AzureKeyVault:TenantId"],
                configuration["AzureKeyVault:ClientId"],
                configuration["AzureKeyVault:ClienteSecret"]);

            #region Get Secret

            Console.WriteLine("GetSecret");

            Console.WriteLine($"key1: { keyVault.GetSecret("key1")}");
            Console.WriteLine($"key2: { keyVault.GetSecret("key2")}");
            Console.WriteLine($"key3: { keyVault.GetSecret("key3")}");

            Console.WriteLine("");
            #endregion

            #region Get Secrets

            Console.WriteLine("GetSecrets");

            foreach (var item in keyVault.GetSecrets())
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }

            Console.WriteLine("");

            #endregion

            #region Create Update Secret

            Console.WriteLine("Create and Update Secret");

            keyVault.CreateUpdateSecret("key4", "novo valor");

            Console.WriteLine($"key4: { keyVault.GetSecret("key3")}");

            #endregion

            #region Delete Secret And Purge

            keyVault.DeleteSecretAndPurge("key1");

            Console.WriteLine($"key1: { keyVault.GetSecret("key1")}");
            Console.WriteLine();

            //await keyVault.DeleteAndPurgeSecretAsync("Key1");

            #endregion

            #region Update Secret Properties

            string contentType = "text/plan";

            DateTimeOffset expiresOn = new DateTimeOffset(2021, 6, 7, 0, 0, 0, new TimeSpan(1, 0, 0));

            DateTimeOffset notBefore = new DateTimeOffset(2021, 5, 10, 0, 0, 0, new TimeSpan(1, 0, 0));

            Dictionary<string, string> tags = new Dictionary<string, string>();
            tags.Add("tag1", "minha-tag-1");
            tags.Add("tag2", "minha-tag-2");
            tags.Add("tag3", "minha-tag-3");

            keyVault.UpdateSecretProperties("Key1", contentType, expiresOn, notBefore, tags);

            Console.ReadLine();

            #endregion
        }
    }
}
