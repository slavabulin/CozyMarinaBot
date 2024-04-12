using CozyMarinaBot.DAL.Models;

namespace CozyMarinaBot.BLL
{
    internal interface IUsersService
    {
        Task IncrementUsersScoreAsync(User user, CancellationToken cancellationToken);
        string GetStatistics(long chatId);
    }
}