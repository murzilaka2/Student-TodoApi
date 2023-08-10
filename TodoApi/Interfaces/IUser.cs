using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface IUser
    {
        Task<User> GetUserAsync(string id);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> IsUserExistsAsync(string email);
        Task<string> GetUserSaltAsync(string email);
        Task<bool> VerifyUserAsync(User user);

        Task AddUserAsync(User user);
    }
}
