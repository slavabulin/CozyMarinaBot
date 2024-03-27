using Telegram.Bot.Types;

namespace CozyMarinaBot
{
    internal static class Helper
    {
        const string cheerupGifFilePath = "Files/cheerup.gif";

        public static DAL.Models.User ToDalUser(this Telegram.Bot.Types.User user, long chatId = 0)
        {
            return new DAL.Models.User { Id = user.Id, Name = user.FirstName, ChatId = chatId };
        }

        public static InputFileStream GetGifStream()
        {
            FileStream fileStream = new(cheerupGifFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileName = cheerupGifFilePath.Split(Path.DirectorySeparatorChar).Last();

            return new InputFileStream(fileStream, fileName);
        }
    }
}
