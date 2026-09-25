using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Infrastructure.Context;
using UrlShortener.Infrastructure.DomainModels;
using UrlShortener.Models;

namespace UrlShortener.Infrastructure.Repositories
{
    internal class AuthRepository(UrlShortenerDbContext db, UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher) : IAuthRepository
    {
        public async Task<bool> UserExistsByEmailOrUsername(string email, string userName)
        {
            return await db.Users.AnyAsync(u => u.Email == email || u.UserName == userName);
        }

        public async Task CreateUser(User newUser, string password)
        {
            ApplicationUser newDbUser = new ApplicationUser
            {
                UserName = newUser.UserName
            };

            IdentityResult dbResult = await userManager.CreateAsync(newDbUser, password);


            if (!dbResult.Succeeded)
            {
                string errors = string.Join(", ", dbResult.Errors);
                throw ErrorException.BadRequest(errors);
            }
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            ApplicationUser dbUser = await db.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (dbUser == null) throw ErrorException.NotFound(userName);

            User user = new User
            {
                ID = dbUser.Id,
                UserName = dbUser.UserName,
                Email = dbUser.Email
            };

            return user;
        }

        public async Task<bool> CheckPassword(Guid userId, string password)
        {
            ApplicationUser applicationUser = await userManager.FindByIdAsync(userId.ToString());

            var result = passwordHasher.VerifyHashedPassword(applicationUser, applicationUser.PasswordHash, password);

            return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}