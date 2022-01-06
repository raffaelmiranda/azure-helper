using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Azure.Helper.ActiveDirectory
{
    public interface IActiveDirectoryAzure
    {
        Task<IList<UserDto>> GetAllUsersByNameAsync(string name);
        Task<UserDto> GetUsersByIdAsync(Guid id);
        Task<UserDto> GetByEmail(string memberEmail);
    }
}
