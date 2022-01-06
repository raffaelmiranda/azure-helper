using System;

namespace Azure.Helper.ActiveDirectory
{
    public class UserDto
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTimeOffset? CreatedDateTime { get; private set; }
        public string UserType { get; private set; }
        public string JobTitle { get; private set; }
        public UserDto(Guid id, string name, string email, DateTimeOffset? createdDateTime, string userType, string jobTitle)
        {
            Id = id;
            Name = name;
            Email = email;
            CreatedDateTime = createdDateTime;
            UserType = userType;
            JobTitle = jobTitle;
        }
    }
}
