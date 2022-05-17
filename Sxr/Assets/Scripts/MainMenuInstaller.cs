using Presenter.MainMenu;
using UnityEngine;
using View.MainMenu;

namespace DefaultNamespace
{
    internal class MainMenuInstaller : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenuUIPrefab;
        [SerializeField] private DifficultyManager _difficultyManager;
        private void Awake()
        {
            var mainMenuView = Instantiate(_mainMenuUIPrefab).GetComponent<MainMenuView>();
            var mainMenuPresenter = new MainMenuPresenter(_difficultyManager, mainMenuView);
            mainMenuPresenter.Init();
        }
    }
}