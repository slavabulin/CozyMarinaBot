using CozyMarinaBot.DAL.Models;

namespace CozyMarinaBot.DAL.Services
{
    internal interface IUsersService
    {
        Task IncrementUsersScoreAsync(User user, CancellationToken cancellationToken);
        string GetStatistics(long chatId);
    }
}