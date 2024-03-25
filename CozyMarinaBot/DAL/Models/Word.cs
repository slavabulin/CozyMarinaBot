using Microsoft.EntityFrameworkCore;

namespace CozyMarinaBot.DAL.Models
{
    [PrimaryKey(nameof(Id))]
    internal class Word
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
