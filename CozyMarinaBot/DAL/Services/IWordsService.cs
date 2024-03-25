
namespace CozyMarinaBot.DAL.Services
{
    internal interface IWordsService
    {
        Task<string> GetWordAsync();
    }
}