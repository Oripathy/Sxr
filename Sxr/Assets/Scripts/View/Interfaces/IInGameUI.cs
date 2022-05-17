using System;

namespace View.Interfaces
{
    internal interface IInGameUI
    {
        public event Action PauseButtonPressed;
        public event Action RestartButtonPressed;
        public event Action RebuildButtonPressed;
        public event Action MainMenuButtonPressed;
        public event Action ResumeButtonPressed;
        public event Action NextLevelButtonPressed;

        public void SetInGameMenuActive(bool isActive);
        public void UpdateUnitsAmountText(int amount);
        public void UpdateSwipesAmountLeftText(int amount);
        public void UpdateTurnText(string turn);
        public void UpdateUnitsAmountSavedText(int amount);
        public void ConvertMenuToWonMenu();
        public void ConvertMenuToLostMenu();
    }
}