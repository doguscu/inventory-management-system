using System.Security.Cryptography;
using System.Text;
using InventoryApp.Data;
using InventoryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Services
{
    public class UserService
    {
        private readonly InventoryDbContext _context;

        public UserService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUserAsync(string email, string fullName, string password)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

                if (existingUser != null)
                {
                    return false; // User already exists
                }

                // Hash password
                string passwordHash = HashPassword(password);

                // Create new user
                var user = new User
                {
                    Email = email.ToLower(),
                    FullName = fullName,
                    PasswordHash = passwordHash,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.IsActive);

                if (user == null)
                {
                    return null;
                }

                // Verify password
                if (VerifyPassword(password, user.PasswordHash))
                {
                    // Update last login date
                    user.LastLoginDate = DateTime.Now;
                    await _context.SaveChangesAsync();

                    return user;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                return await _context.Users
                    .AnyAsync(u => u.Email.ToLower() == email.ToLower());
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "InventoryAppSalt"));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            string hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }
    }
}