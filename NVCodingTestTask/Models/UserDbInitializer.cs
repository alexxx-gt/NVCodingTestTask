using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NVCodingTestTask.Models.UserDbInitializer))]

namespace NVCodingTestTask.Models
{
    public class UserDbInitializer : DropCreateDatabaseAlways<UserContext>
    {
        UserContext db = new UserContext();

        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }

        protected override void Seed(UserContext context)
        {
            List<User> seedingUsersList = new List<User>();
            seedingUsersList.Add(new User{Email = "ivanov@gmail.com", Name = "IvanovIvan", Avatar = "\\Users\\Avatars\\ivanov.png", SkypeLogin = "Ivanov2017", Signature = "ABCDEF0123456789"});
            seedingUsersList.Add(new User { Email = "petrov@outlook.com", Name = "PetrovPetr", Avatar = "\\Users\\Avatars\\petrov.png", SkypeLogin = "Petrov2017", Signature = "0123456789ABCDEF" });
            seedingUsersList.Add(new User { Email = "sidorov@hotmail.com", Name = "SidorovSidor", Avatar = "\\Users\\Avatars\\sidorov.png", SkypeLogin = "Sidorov2017", Signature = "01234ABCDEF56789" });

            db.Users.AddRange(seedingUsersList);
            db.SaveChangesAsync();

            base.Seed(context);
        }
    }
}
