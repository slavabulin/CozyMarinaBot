using CozyMarinaBot.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CozyMarinaBot.DAL.Repositories
{
    internal class WordsRepo : IWordsRepo
    {
        private readonly ILogger<WordsRepo> _logger;

        public WordsRepo(ILogger<WordsRepo> logger)
        {
            _logger = logger;
        }
        public async Task<int> GetWordsCountAsync()
        {
            var count = 0;
            try
            {
                using var db = new Context();
                count = await db.Words.CountAsync();
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            
        }
        public async Task<string> GetRandomWordAsync()
        {
            var rnd = new Random();
            try
            {
                using var db = new Context();
                var wordNumber = await GetWordsCountAsync();
                var randomId = rnd.Next(1, wordNumber + 1);
                var word = await GetWordByIdAsync(randomId);
                return word.Text;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<Word> GetWordByIdAsync(int id)
        {
            try
            {
                using var db = new Context();
                return await db.Words.FirstOrDefaultAsync(w => w.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
