namespace CozyMarinaBot
{
    internal static class Helper
    {
        public static DAL.Models.User ToDalUser(this Telegram.Bot.Types.User user, long chatId = 0)
        {
            return new DAL.Models.User { Id = user.Id, Name = user.FirstName, ChatId = chatId };
        }
    }
}
