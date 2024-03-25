using CozyMarinaBot.DAL.Models;

namespace CozyMarinaBot.DAL.Repositories
{
    internal interface IUsersRepo
    {
        Task<User> GetUserById(long id, long chatId, CancellationToken cancellationToken);
        Statistic[] GetChatStatistics(long chatId);
        Task<User> UpdateUser(User user, CancellationToken cancellationToken);
    }
}