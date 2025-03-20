using FrostWolfHunters.Scripts.Game.Data;
using R3;

namespace FrostWolfHunters.Scripts.Game.MainMenu.Root
{
    public class MainMenuEnterParams
    {
        public string  Result { get; }

        public MainMenuEnterParams(string result)
        {
            Result = result;
        }
    }
}