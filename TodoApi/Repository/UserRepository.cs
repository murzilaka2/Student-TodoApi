using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repository
{
    public class UserRepository : IUser
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(e => e.Email.Equals(email));
        }

        public async Task<string> GetUserSaltAsync(string email)
        {
            return await _context.Users.Where(e => e.Email.Equals(email)).Select(e => e.Salt).FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(e=> e.Email.Equals(email));
        }

        public async Task<bool> VerifyUserAsync(User user)
        {
            return await _context.Users.AnyAsync(e => e.HashedPasssword.Equals(user.HashedPasssword) &&
            e.Email.Equals(user.Email));
        }
    }
}
