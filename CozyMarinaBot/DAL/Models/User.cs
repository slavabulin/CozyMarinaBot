using System.ComponentModel.DataAnnotations;

namespace CozyMarinaBot.DAL.Models
{
    internal class User
    {
        [Key]
        public int UserId {  get; set; } = 0;
        public string UserName { get; set; }
    }
}
