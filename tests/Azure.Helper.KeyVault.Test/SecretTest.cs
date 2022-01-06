using Azure.Helper.KeyVault.Test.Builder;
using System;
using Xunit;

namespace Azure.Helper.KeyVault.Test
{
    public class SecretTest
    {
        private string _keyVaultName = string.Empty;
        private string _tenantId = string.Empty;
        private string _clientId = string.Empty;
        private string _clientSecret = string.Empty;

        public SecretTest()
        {
            _keyVaultName = Config.KEY_VAULT_NAME;
            _tenantId = Config.TENANT_ID;
            _clientId = Config.CLIENT_ID;
            _clientSecret = Config.CLIENT_SECRET;
        }

        [Fact]
        public void DeveCriarInstanciaDeSecret()
        {
            Secret secret = new Secret(_keyVaultName, _tenantId, _clientId, _clientSecret);

            Assert.NotNull(secret);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void KeyVaultNameNaoDeveEstarVazio(string keyVaultName)
        {
            var ex = Assert.Throws<Exception>(() =>
               SecretBuilder
               .Novo()
               .TenantId(_tenantId).ClientId(_clientId).ClientSecret(_clientSecret).KeyVaultName(keyVaultName)
               .Build());

            Assert.Equal("KeyVaultName é obrigatório", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TenantIdNaoDeveEstarVazio(string tenantId)
        {
            var ex = Assert.Throws<Exception>(() =>
                SecretBuilder
                .Novo()
                .TenantId(tenantId).ClientId(_clientId).ClientSecret(_clientSecret).KeyVaultName(_keyVaultName)
                .Build());

            Assert.Equal("TenantId é obrigatório", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ClientIdNaoDeveEstarVazio(string clientId)
        {
            var ex = Assert.Throws<Exception>(() =>
              SecretBuilder
              .Novo()
              .TenantId(_tenantId).ClientId(clientId).ClientSecret(_clientSecret).KeyVaultName(_keyVaultName)
              .Build());

            Assert.Equal("ClientId é obrigatório", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ClientSecretNaoDeveEstarVazio(string clientSecret)
        {
            var ex = Assert.Throws<Exception>(() =>
              SecretBuilder
              .Novo()
              .TenantId(_tenantId).ClientId(_clientId).ClientSecret(clientSecret).KeyVaultName(_keyVaultName)
              .Build());

            Assert.Equal("ClientSecret é obrigatório", ex.Message);
        }

        [Fact]
        public void DeveCriarSecret()
        {
            var secret = SecretBuilder.Novo().Build();

            secret.CreateUpdateSecret("Key1", "minha-chave1");
            string valorChave = secret.GetSecret("Key1");

            Assert.Equal("minha-chave1", valorChave);
        }

        //[Fact]
        //public void NaoCriaNovaSecretComMesmoNomeDeSecretExcluido()
        //{
        //    var secret = SecretBuilder.Novo().Build();

        //    secret.CreateUpdateSecret("Key1", "minha-chave1");
        //    string valorChave = secret.GetSecret("Key1");

        //    Assert.Equal("minha-chave1", valorChave);
        //}

    }
}
