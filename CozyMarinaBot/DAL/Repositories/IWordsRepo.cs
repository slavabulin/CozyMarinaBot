using CozyMarinaBot.DAL.Models;

namespace CozyMarinaBot.DAL.Repositories
{
    internal interface IWordsRepo
    {
        Task<string> GetRandomWordAsync();
        Task<Word> GetWordByIdAsync(int id);
    }
}