using System;

namespace View.MainMenu
{
    internal interface IMainMenuView
    {
        public event Action StartButtonPressed;
        public event Action QuitButtonPressed;

        public void SetCurrentDifficultyText(int difficulty);
    }
}