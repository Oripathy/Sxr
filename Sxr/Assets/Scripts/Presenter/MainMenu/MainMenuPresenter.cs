using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Model.Game;
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
            var difficulty = LoadDifficulty();
            _view.SetCurrentDifficultyText(difficulty);
            _view.StartButtonPressed += OnStartButtonPressed;
            _view.QuitButtonPressed += OnQuitButtonPressed;
        }

        private void OnStartButtonPressed() => SceneManager.LoadScene("Scenes/SampleScene");

        private void OnQuitButtonPressed() => Application.Quit();

        private int LoadDifficulty()
        {
            string path = Path.Combine(Application.persistentDataPath, "data.oi");
            
            if (File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                var data = binaryFormatter.Deserialize(stream) as Data;
                stream.Close();
                return data.achievedDifficulty;
            }

            Debug.Log("Save file not found");
            return 1;
        }
        
    }
}