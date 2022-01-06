namespace Azure.Helper.KeyVault.Test.Builder
{
    public class SecretBuilder
    {
        private string _keyVaultName = string.Empty;
        private string _tenantId = string.Empty;
        private string _clientId = string.Empty;
        private string _clientSecret = string.Empty;

        public static SecretBuilder Novo()
        {
            return new SecretBuilder();
        }

        public SecretBuilder TenantId(string tenantId)
        {
            _tenantId = tenantId;
            return this;
        }

        public SecretBuilder ClientId(string clientId)
        {
            _clientId = clientId;
            return this;
        }

        public SecretBuilder ClientSecret(string clientSecret)
        {
            _clientSecret = clientSecret;
            return this;
        }

        public SecretBuilder KeyVaultName(string keyVaultName)
        {
            _keyVaultName = keyVaultName;
            return this;
        }

        public Secret Build()
        {
            var secret = new Secret(_keyVaultName, _tenantId, _clientId, _clientSecret);

            return secret;
        }

    }
}
