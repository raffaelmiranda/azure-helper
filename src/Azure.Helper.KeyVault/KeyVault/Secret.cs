using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Azure.Helper.KeyVault
{
    public class Secret
    {
        private TokenCredential _token = null;
        private SecretClient _client = null;
        private string _keyVaultUrl = string.Empty;
        private string _keyVaultName = string.Empty;
        private string _tenantId = string.Empty;
        private string _clientId = string.Empty;
        private string _clientSecret = string.Empty;

        public Secret(string keyVaultName)
        {

            _token = new DefaultAzureCredential(false);
            _client = new SecretClient(new Uri($"https://{keyVaultName}.vault.azure.net/"), _token);
        }

        public Secret(string keyVaultName, string tenantId, string clientId, string clientSecret)
        {

            if (string.IsNullOrEmpty(keyVaultName))
                throw new Exception("KeyVaultName é obrigatório");

            if (string.IsNullOrEmpty(tenantId))
                throw new Exception("TenantId é obrigatório");

            if (string.IsNullOrEmpty(clientId))
                throw new Exception("ClientId é obrigatório");

            if (string.IsNullOrEmpty(clientSecret))
                throw new Exception("ClientSecret é obrigatório");

            _keyVaultName = keyVaultName;
            _tenantId = tenantId;
            _clientId = clientId;
            _clientSecret = clientSecret;

            _keyVaultUrl = $"https://{keyVaultName}.vault.azure.net/";

            _token = new ClientSecretCredential(_tenantId, _clientId, _clientSecret);
            _client = new SecretClient(new Uri(_keyVaultUrl), _token);
        }
        
        public string GetSecret(string secretName)
        {
            if (string.IsNullOrEmpty(secretName))
                throw new Exception("O nome da secret está vazio");

            try
            {
                KeyVaultSecret secret = _client.GetSecret(secretName).Value;
                return secret.Value;
            }
            catch (RequestFailedException ex)
            {
                if (ex.Status == (int)HttpStatusCode.NotFound)
                    throw new Exception($"A secret {secretName} não existe em {_keyVaultName}");

                return ex.Message;
            }
        }
        
        public Dictionary<string, string> GetSecrets()
        {
            Dictionary<string, string> secrets = new Dictionary<string, string>();

            foreach (var secretProperty in _client.GetPropertiesOfSecrets())
            {
                KeyVaultSecret secret  = _client.GetSecret(secretProperty.Name);

                secrets.Add(secret.Name, secret.Value);
            }

            return secrets;
        }

        public void CreateUpdateSecret(string secretName, string value)
        {
            if (string.IsNullOrEmpty(secretName))
                throw new Exception("O nome da secret está vazio");

            if (string.IsNullOrEmpty(value))
                throw new Exception("O valor da secret está vazio");

            try
            {
                KeyVaultSecret secret = new KeyVaultSecret(secretName, value);
                _client.SetSecret(secret);

            }
            catch (RequestFailedException ex)
            {
                if (ex.Status == (int)HttpStatusCode.InternalServerError)
                    throw new Exception($"Erro ao criar a secret {secretName} em {_keyVaultName}");

                if (ex.Status == (int)HttpStatusCode.Conflict)
                    throw new Exception($"Secret {secretName} está atualmente em um estado excluído, mas recuperável, " +
                        $"e seu nome não pode ser reutilizado; nesse estado, o segredo só pode ser recuperado ou eliminado.");
            }
        }

        public void DeleteSecretAndPurge(string secretName)
        {
            DeleteSecretOperation operation = _client.StartDeleteSecret(secretName);
            while (!operation.HasCompleted)
            {
                operation.UpdateStatus();
            }

            _client.PurgeDeletedSecret(secretName);
        }

        public async Task DeleteAndPurgeSecretAsync(string secretName)
        {
            DeleteSecretOperation operation = await _client.StartDeleteSecretAsync(secretName);

            await operation.WaitForCompletionAsync();

            DeletedSecret secret = operation.Value;
            await _client.PurgeDeletedSecretAsync(secret.Name);
        }

        public bool UpdateSecretProperties(string secretName, string contentType = null,
            DateTimeOffset? expiresOn = null, DateTimeOffset? notBefore = null, Dictionary<string, string> tags = null)
        {
            try
            {
                KeyVaultSecret secret = _client.GetSecret(secretName);
                //secret.Properties.Enabled = enable;
                secret.Properties.ContentType = contentType;
                secret.Properties.ExpiresOn = expiresOn;
                secret.Properties.NotBefore = notBefore;

                foreach (var item in tags)
                    secret.Properties.Tags[item.Key] = item.Value;


                Response<SecretProperties> updatedSecretProperties = _client.UpdateSecretProperties(secret.Properties);

                if (updatedSecretProperties.GetRawResponse().Status == 200)
                    return true;
                else
                    return false;
            }
            catch (RequestFailedException ex)
            {
                return false;
            }
           

        }
    }
}
