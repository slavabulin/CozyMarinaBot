using CozyMarinaBot.DAL.Models;
using CozyMarinaBot.DAL.Repositories;
using System.Text;

namespace CozyMarinaBot.DAL.Services
{
    internal class UsersService : IUsersService
    {
        private readonly IUsersRepo _usersRepo;
        private readonly ILogger<UsersService> _logger;

        public UsersService(IUsersRepo usersRepo, ILogger<UsersService> logger)
        {
            _usersRepo = usersRepo;
            _logger = logger;
        }

        public async Task IncrementUsersScoreAsync(User user, CancellationToken cancellationToken)
        {
            var dalUser = await _usersRepo.GetUserById(user.Id, user.ChatId, cancellationToken);
            if (dalUser == null)
            {
                user.Score = 1;
            }
            else
            {
                user.Score = dalUser.Score + 1;
            }
            await _usersRepo.UpdateUser(user, cancellationToken);
        }

        public string GetStatistics(long chatId)
        {
            var statArr = _usersRepo.GetChatStatistics(chatId);
            var sb = new StringBuilder();
            foreach (var stat in statArr)
            {
                sb.AppendLine($"{stat.Name} - {stat.Score}\n");
            }
            return sb.ToString();
        }
    }
}
