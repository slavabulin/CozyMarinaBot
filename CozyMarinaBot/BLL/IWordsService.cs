namespace CozyMarinaBot.BLL
{
    internal interface IWordsService
    {
        Task<string> GetWordAsync();
    }
}