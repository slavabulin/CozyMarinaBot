namespace CozyMarinaBot.Services
{
    internal class ChatData
    {
        public bool GameIsStarted { get; set; } = false;
        public string SecretWord { get; set; } = string.Empty;
        public long? HostId { get; set; }
        public bool WordIsChosen { get; set; } = false;
    }
}
