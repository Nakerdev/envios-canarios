using System;
using LanguageExt;

namespace CanaryDeliveries.Backoffice.Api.Users.Login.Repositories
{
    public interface BackofficeUserRepository
    {
        Option<BackofficeUser> SearchBy(string email);
    }

    public sealed class BackofficeUser
    {
        public Guid Id { get; }
        public string Email { get; }
        public string Password { get; }

        public BackofficeUser(
            Guid id, 
            string email, 
            string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }
    }
}