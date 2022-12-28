using Azure.Helper.KeyVault;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

Secret keyVault = new Secret("");

GetSecret(keyVault);

GetSecrets(keyVault);

CreateUpdateSecret(keyVault);

DeleteSecretAndPurge(keyVault);

UpdateSecretProperties(keyVault);


void GetSecret(Secret keyVault)
{
    Console.WriteLine("GetSecret");

    Console.WriteLine($"key1: {keyVault.GetSecret("key1")}");
    Console.WriteLine($"key2: {keyVault.GetSecret("key2")}");
    Console.WriteLine($"key3: {keyVault.GetSecret("key3")}");

    Console.WriteLine("");
}

void GetSecrets(Secret keyVault)
{
    Console.WriteLine("GetSecrets");

    foreach (var item in keyVault.GetSecrets())
    {
        Console.WriteLine($"{item.Key}: {item.Value}");
    }

    Console.WriteLine("");
}

void CreateUpdateSecret(Secret keyVault)
{
    Console.WriteLine("Create and Update Secret");

    keyVault.CreateUpdateSecret("key4", "novo valor");

    Console.WriteLine($"key4: {keyVault.GetSecret("key3")}");
}

void DeleteSecretAndPurge(Secret keyVault)
{
    keyVault.DeleteSecretAndPurge("key1");

    Console.WriteLine($"key1: {keyVault.GetSecret("key1")}");
    Console.WriteLine();

    //await keyVault.DeleteAndPurgeSecretAsync("Key1");
}

void UpdateSecretProperties(Secret keyVault)
{
    string contentType = "text/plan";

    DateTimeOffset expiresOn = new DateTimeOffset(2021, 6, 7, 0, 0, 0, new TimeSpan(1, 0, 0));

    DateTimeOffset notBefore = new DateTimeOffset(2021, 5, 10, 0, 0, 0, new TimeSpan(1, 0, 0));

    Dictionary<string, string> tags = new Dictionary<string, string>();
    tags.Add("tag1", "minha-tag-1");
    tags.Add("tag2", "minha-tag-2");
    tags.Add("tag3", "minha-tag-3");

    keyVault.UpdateSecretProperties("Key1", contentType, expiresOn, notBefore, tags);

    Console.ReadLine();
}
