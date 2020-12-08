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
                    password: "$2y$12$T3VBkao8WEbPxagqYoBih.R.dViAKz.MokzYMc90iSr.AhpNNgLZO")
            });
        }

        public Option<BackofficeUser> SearchBy(string email)
        {
            return users.FirstOrDefault(x => x.Email == email);
        }
    }
}