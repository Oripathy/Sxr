using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.MainMenu
{
    internal class MainMenuView : MonoBehaviour, IMainMenuView
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private TMP_Text _currentDifficultyText;

        public event Action StartButtonPressed;
        public event Action QuitButtonPressed;
        
        private void Start()
        {
            _startButton.onClick.AddListener(() => StartButtonPressed?.Invoke());
            _quitButton.onClick.AddListener(() => QuitButtonPressed?.Invoke());
        }

        public void SetCurrentDifficultyText(int difficulty) =>
            _currentDifficultyText.text = "Current difficulty : " + difficulty;
    }
}