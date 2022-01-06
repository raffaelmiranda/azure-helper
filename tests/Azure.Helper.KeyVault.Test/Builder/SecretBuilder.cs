namespace Azure.Helper.KeyVault.Test.Builder
{
    public class SecretBuilder
    {
        private string _keyVaultName = Config.KEY_VAULT_NAME;
        private string _tenantId = Config.TENANT_ID;
        private string _clientId = Config.CLIENT_ID;
        private string _clientSecret = Config.CLIENT_SECRET;

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
