
using TodoApi.Helpers;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class DbInit
    {
        public static async Task InitializeAsync(IUser users)
        {
            if (!await users.IsUserExistsAsync("admin@gmail.com"))
            {
                string password = "qwerty";
                string salt = SecurityHelper.GenerateSalt(70);
                string hashedPassword = SecurityHelper.HashPassword(password, salt, 10101, 70);

                User user = new User
                {
                    Email = "admin@gmail.com",
                    Salt = salt,
                    HashedPasssword = hashedPassword,
                };
                await users.AddUserAsync(user);
            }
            if (!await users.IsUserExistsAsync("alex@gmail.com"))
            {
                string password = "123456789";
                string salt = SecurityHelper.GenerateSalt(70);
                string hashedPassword = SecurityHelper.HashPassword(password, salt, 10101, 70);

                User user = new User
                {
                    Email = "alex@gmail.com",
                    Salt = salt,
                    HashedPasssword = hashedPassword,
                };
                await users.AddUserAsync(user);
            }
        }
    }
}
