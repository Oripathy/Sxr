using UnityEngine;
using UnityEngine.SceneManagement;
using View.MainMenu;

namespace Presenter.MainMenu
{
    internal class MainMenuPresenter
    {
        private IMainMenuView _view;
        private DifficultyManager _difficultyManager;

        public MainMenuPresenter(DifficultyManager difficultyManager, IMainMenuView view)
        {
            _difficultyManager = difficultyManager;
            _view = view;
        }

        public void Init()
        {
            _view.SetCurrentDifficultyText(_difficultyManager.CurrentDifficulty);
            _view.StartButtonPressed += OnStartButtonPressed;
            _view.QuitButtonPressed += OnQuitButtonPressed;
        }

        private void OnStartButtonPressed() => SceneManager.LoadScene("Scenes/SampleScene");

        private void OnQuitButtonPressed() => Application.Quit();
    }
}