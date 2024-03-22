using CozyMarinaBot.DAL.Models;

namespace CozyMarinaBot.DAL.Repositories
{
    internal class UserRepo
    {
        public async Task AddUserAsync(User user)
        {
            using var db = new Context();
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
        }
    }
}
