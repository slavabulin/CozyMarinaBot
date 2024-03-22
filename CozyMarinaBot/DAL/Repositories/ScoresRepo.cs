namespace CozyMarinaBot.DAL.Repositories
{
    internal class ScoresRepo
    {
        public async Task AddScoreAsync(int  id, int score)
        {
            using var db = new Context();
            await db.Scores.AddAsync(new Models.Score { UserId = id, Value = score });
            await db.SaveChangesAsync();
        }
    }
}
