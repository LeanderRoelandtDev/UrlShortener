using UrlShortener.Models;

namespace UrlShortener.Core.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> UserExistsByEmailOrUsername(string email, string userName);
        Task CreateUser(User newUser, string password);
        Task<User> GetUserByUserName(string userName);
        Task<bool> CheckPassword(Guid userId, string password);
    }
}