using CozyMarinaBot.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CozyMarinaBot.DAL.Repositories
{
    internal class WordsRepo
    {
        static async Task<int> GetWordsCountAsync()
        {
            var count = 0;
            using(var db = new Context())
            {
                count = await db.Words.CountAsync();
            }
            return count;
        }
        public static async Task<string> GetRandomWordAsync()
        {
            var rnd = new Random();
            using var db = new Context();
            var wordNumber = await GetWordsCountAsync();
            var randomId = rnd.Next(1, wordNumber + 1);
            var word = await GetWordByIdAsync(randomId);
            return word.Text;
        }
        public static async Task<Word> GetWordByIdAsync(int id)
        {
            using var db = new Context();
            return await db.Words.FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}
