using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Interfaces;

namespace View.InGameUI
{
    internal class InGameUI : MonoBehaviour, IInGameUI
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _rebuildButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private TMP_Text _menuTitleText;
        [SerializeField] private TMP_Text _unitsAmountText;
        [SerializeField] private TMP_Text _swipesAmountLeftText;
        [SerializeField] private TMP_Text _turnText;
        [SerializeField] private TMP_Text _unitsAmountSavedText;
        
        public event Action PauseButtonPressed;
        public event Action RestartButtonPressed;
        public event Action RebuildButtonPressed;
        public event Action MainMenuButtonPressed;
        public event Action ResumeButtonPressed;
        public event Action NextLevelButtonPressed;
        
        private void Start()
        {
            _pauseMenu.SetActive(false);
            _pauseButton.enabled = true;
            _resumeButton.gameObject.SetActive(true);
            _restartButton.gameObject.SetActive(true);
            _rebuildButton.gameObject.SetActive(true);
            _nextLevelButton.gameObject.SetActive(false);
            _menuTitleText.text = "Pause";
            _pauseButton.onClick.AddListener(() => PauseButtonPressed?.Invoke());
            _restartButton.onClick.AddListener(() => RestartButtonPressed?.Invoke());
            _rebuildButton.onClick.AddListener(() => RebuildButtonPressed?.Invoke());
            _mainMenuButton.onClick.AddListener(() => MainMenuButtonPressed?.Invoke());
            _resumeButton.onClick.AddListener(() => ResumeButtonPressed?.Invoke());
            _nextLevelButton.onClick.AddListener(() => NextLevelButtonPressed?.Invoke());
        }

        public void SetInGameMenuActive(bool isActive) => _pauseMenu.SetActive(isActive);
        
        public void UpdateUnitsAmountText(int amount) => _unitsAmountText.text = amount + "/5";

        public void UpdateSwipesAmountLeftText(int amount) => _swipesAmountLeftText.text = amount.ToString();

        public void UpdateTurnText(string turn) => _turnText.text = turn;

        public void UpdateUnitsAmountSavedText(int amount) => _unitsAmountSavedText.text = amount.ToString();

        public void ConvertMenuToWonMenu()
        {
            SetInGameMenuActive(true);
            _resumeButton.gameObject.SetActive(false);
            _restartButton.gameObject.SetActive(false);
            _rebuildButton.gameObject.SetActive(false);
            _nextLevelButton.gameObject.SetActive(true);
            _pauseButton.enabled = false;
            _menuTitleText.text = "You won";
        }

        public void ConvertMenuToLostMenu()
        {
            SetInGameMenuActive(true);
            _resumeButton.gameObject.SetActive(false);
            _restartButton.gameObject.SetActive(true);
            _rebuildButton.gameObject.SetActive(true);
            _nextLevelButton.gameObject.SetActive(false);
            _pauseButton.enabled = false;
            _menuTitleText.text = "You lost";
        }
    }
}