using Microsoft.EntityFrameworkCore;

namespace CozyMarinaBot.DAL.Models
{
    [PrimaryKey(nameof(Id), nameof(ChatId))]
    internal class User
    {
        public long Id {  get; set; } = 0;
        public long ChatId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; } = 0;
    }
}
