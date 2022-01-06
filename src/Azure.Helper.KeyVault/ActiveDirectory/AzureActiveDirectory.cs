using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Helper.ActiveDirectory
{
    public class AzureActiveDirectory: IActiveDirectoryAzure
    {
        private readonly GraphServiceClient _graphClient;

        public AzureActiveDirectory(string tenantId, string clientId, string clientSecret)
        {
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);
            _graphClient = new GraphServiceClient(clientSecretCredential, scopes); 
        }

        public async Task<IList<UserDto>> GetAllUsersByNameAsync(string name)
        {
            IGraphServiceUsersCollectionPage users;

            if (!string.IsNullOrEmpty(name))
            {
                users = await _graphClient.Users
               .Request()
               .Filter($"startsWith(displayName,'{name}')")
               .Select("id,displayName,mail,accountEnabled,createdDateTime,userType,jobTitle")
               .GetAsync();
            }
            else
            {
                users = await _graphClient.Users
               .Request()
               .Select("id,displayName,mail,accountEnabled,createdDateTime,userType,jobTitle")
               .GetAsync();
            }


            var usersDto = users.CurrentPage.ToList()
                .Select(s => new UserDto(Guid.Parse(s.Id), s.DisplayName, s.Mail, s.CreatedDateTime, s.UserType, s.JobTitle))
                .OrderBy(o => o.Name)
                .ToList();

            return usersDto;
        }

        public async Task<UserDto> GetByEmail(string memberEmail)
        {
            var users = await _graphClient.Users
              .Request()
              .Filter($"startsWith(mail,'{memberEmail}')")
              .Select("id,displayName,mail,accountEnabled,createdDateTime,userType,jobTitle")
              .GetAsync();

            var userAd = users.CurrentPage.FirstOrDefault();
            var userDto = new UserDto(Guid.Parse(userAd?.Id), userAd?.DisplayName, userAd.Mail, userAd?.CreatedDateTime, userAd?.UserType, userAd?.JobTitle);
            return userDto;
        }

        public async Task<UserDto> GetUsersByIdAsync(Guid id)
        {
            var users = await _graphClient.Users
               .Request()
               .Filter($"id eq '{id}'")
               .Select("id,displayName,mail,accountEnabled,createdDateTime,userType,jobTitle")
               .GetAsync();

            var userAd = users.CurrentPage.FirstOrDefault();
            var userDto = new UserDto(Guid.Parse(userAd?.Id), userAd?.DisplayName, userAd.Mail, userAd?.CreatedDateTime, userAd?.UserType, userAd?.JobTitle);
            return userDto;
        }
    }
}
