namespace CozyMarinaBot.Services
{
    internal class BearService : IBearService
    {
        string[] _bears = {"・㉨・", "తꀧత", "ᵔᴥᵔ", "ʕ•ᴥ•ʔ", "ʕ·ᴥ·ʔ", "ˁ˙˟˙ˀ", "❃ႣᄎႣ❃", "ʕ　·ᴥʔ", "ʕᴥ·　ʔ", "ʕథ౪థʔ",
            "ʕ￫ᴥ￩ʔ", "(๏㉨๏)", "(ó㉨ò)", "ʢᵕᴗᵕʡ", "ʕ◉ᴥ◉ʔ", "ʕᴥ• ʔ", "ʕ≧ᴥ≦ʔ", "ʕ•㉨•ʔ", "ʕ≧㉨≦ʔ", "(✪㉨✪)", "ʕ∙ჲ∙ʔ",
            "ʕʽɞʼʔ", "[｡◉㉨◉]", "ʕº̫͡ºʔ", "ʕ·ᴥ·　ʔ", "ʕ*̫͡*ʔ", "ʕ•̮͡•ʔ", "ᶘ ᵒᴥᵒᶅ"};
        public string GetNewBear()
        {
            var curTime = DateTime.Now;
            var index = curTime.Ticks % _bears.Length;

            return _bears[index];
        }
    }
}
