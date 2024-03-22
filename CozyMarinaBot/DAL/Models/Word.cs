using System.ComponentModel.DataAnnotations;

namespace CozyMarinaBot.DAL.Models
{
    internal class Word
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
