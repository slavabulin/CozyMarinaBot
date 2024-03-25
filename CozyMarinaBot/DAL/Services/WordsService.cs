using CozyMarinaBot.DAL.Repositories;

namespace CozyMarinaBot.DAL.Services
{
    internal class WordsService : IWordsService
    {
        private readonly IWordsRepo _wordsRepo;
        private readonly ILogger<WordsService> _logger;

        public WordsService(IWordsRepo wordsRepo, ILogger<WordsService> logger)
        {
            _wordsRepo = wordsRepo;
            _logger = logger;
        }
        public async Task<string> GetWordAsync()
        {
            return await _wordsRepo.GetRandomWordAsync();
        }
    }
}
