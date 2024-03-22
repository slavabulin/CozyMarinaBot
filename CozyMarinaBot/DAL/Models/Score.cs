using System.ComponentModel.DataAnnotations;

namespace CozyMarinaBot.DAL.Models
{
    internal class Score
    {
        [Key]
        public int UserId { get; set; } = 0;
        public int Value {  get; set; }
    }
}
