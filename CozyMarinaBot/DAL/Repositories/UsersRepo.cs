using CozyMarinaBot.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CozyMarinaBot.DAL.Repositories
{
    internal class UsersRepo : IUsersRepo
    {
        private readonly ILogger<UsersRepo> _logger;

        public UsersRepo(ILogger<UsersRepo> logger)
        {
            _logger = logger;
        }

        public async Task<User> GetUserById(long id, long chatId, CancellationToken cancellationToken)
        {
            try
            {
                using var db = new Context();
                return await db.Users.FirstOrDefaultAsync(u => u.Id == id && u.ChatId == chatId, cancellationToken);
            }catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
            
        }

        public Statistic[] GetChatStatistics(long chatId)
        {
            try
            {
                using var db = new Context();
                return db.Users
                    .Where(u=>u.ChatId==chatId)
                    .Select(u=>new Statistic { Name = u.Name, Score = u.Score})
                    .OrderByDescending(u=>u.Score)
                    .ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        public async Task<User> UpdateUser(User user, CancellationToken cancellationToken)
        {
            try
            {
                using var db = new Context();
                var dalUser = await db.Users.FirstOrDefaultAsync(u => u.Id == user.Id && u.ChatId == user.ChatId, cancellationToken);
                if (dalUser is null)
                {
                    await db.Users.AddAsync(user, cancellationToken);
                }
                else
                {
                    dalUser.Name = user.Name;
                    dalUser.Score = user.Score;
                }
                bool hasChanges = db.ChangeTracker.HasChanges();
                await db.SaveChangesAsync(cancellationToken);

                return user;
            }catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
            
        }
    }
}
