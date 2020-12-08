using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LanguageExt;

namespace CanaryDeliveries.Backoffice.Api.Users.Login.Repositories
{
    public sealed class BackofficeUserStaticRepository : BackofficeUserRepository
    {
        private readonly ReadOnlyCollection<BackofficeUser> users;

        public BackofficeUserStaticRepository()
        {
            users = new ReadOnlyCollection<BackofficeUser>(new List<BackofficeUser>
            {
                new BackofficeUser(
                    id: new Guid("4aaa7d4f-84eb-4450-92d5-d6755e13b26b"),
                    email: "operador@envioscanarios.com",
                    password: "8a27b0d2095511b26390dd487e1eb641b0c74b4f2daa089855de610356c3e2256a1eb26700aca18f890f551526363356ca0ef6f9f4ecb60b36e8751a1d074e8b")
            });
        }

        public Option<BackofficeUser> SearchBy(string email)
        {
            return users.FirstOrDefault(x => x.Email == email);
        }
    }
}