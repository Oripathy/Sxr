using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using View;

namespace Model.Game
{
    [Serializable]
    internal class Data
    {
        public int totalUnitsAmountSaved;
        public int achievedDifficulty;
    }
    
    internal class GameModel
    {
        private UpdateHandler _updateHandler;
        private DifficultyManager _difficultyManager;
        private GameState _currentState;
        private Dictionary<Type, GameState> _statesByType;
        private bool _isInputActive;
        private bool _isPaused;
        private int _totalUnitsAmountSaved;
        private int _unitsAmountSavedAtThisSession;
        private int _maxSwipes = 5;
        private int _swipesAmountLeft;

        public bool IsInputActive
        {
            get => _isInputActive;
            set
            {
                _isInputActive = value;
                InputActiveStateChanged?.Invoke(_isInputActive);
            }
        }
        
        public bool IsSwipeReceived { get; set; }
        public int SwipesAmountLeft => _swipesAmountLeft;

        public event Action<Type> StateSwitched;
        public event Action<bool> InputActiveStateChanged;
        public event Action<int> SwipesAmountChanged;
        public event Action<int> MaxSwipesAmountDecreased;
        public event Action<int> UnitsAmountSavedChanged;
        public event Action<bool> PauseStateChanged;
        public event Action LevelRestarted;
        public event Action GameLost;
        public event Action GameWon;
        
        public GameModel(DifficultyManager difficultyManager)
        {
            _statesByType = new Dictionary<Type, GameState>
            {
                {typeof(PlayerTurn), new PlayerTurn(this)},
                {typeof(PlayerUnitsMoving), new PlayerUnitsMoving(this)},
                {typeof(EnemyTurn), new EnemyTurn(this)}
            };

            _difficultyManager = difficultyManager;
        }

        public void Init(UpdateHandler updateHandler)
        {
            var savedData = LoadData();
            _totalUnitsAmountSaved = savedData.totalUnitsAmountSaved;
            _difficultyManager.SetDifficulty(savedData.achievedDifficulty);
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += UpdatePass;
            IsInputActive = true;
            ResetSwipesAmountLeft();
            _currentState = _statesByType[typeof(PlayerTurn)];
            _currentState.OnEnter();
            UpdateUI();
        }

        private void UpdateUI()
        {
            SwipesAmountChanged?.Invoke(_swipesAmountLeft);
            MaxSwipesAmountDecreased?.Invoke(_maxSwipes);
            UnitsAmountSavedChanged?.Invoke(_totalUnitsAmountSaved);
            StateSwitched?.Invoke(typeof(PlayerTurn));
        }

        private void UpdatePass()
        {
            _currentState.UpdatePass();
        }

        public void SwitchState<T>()
            where T : GameState
        {
            var type = typeof(T);

            if (_statesByType.TryGetValue(type, out var nextState))
            {
                _currentState.OnExit();
                _currentState = nextState;
                _currentState.OnEnter();
                StateSwitched?.Invoke(type);
            }
        }

        public void RestartLevel()
        {
            _maxSwipes = 5;
            UpdateUI();
            IsInputActive = true;
            _isPaused = false;
            ResetSwipesAmountLeft();
            _currentState = _statesByType[typeof(PlayerTurn)];
            _currentState.OnEnter();
            LevelRestarted?.Invoke();
        }

        public void DecreaseSwipesAmountLeft()
        {
            if (_swipesAmountLeft <= 0)
                return;

            _swipesAmountLeft--;
            SwipesAmountChanged?.Invoke(_swipesAmountLeft);
        }

        public void ResetSwipesAmountLeft()
        {
            if (_swipesAmountLeft > 0)
                return;

            _swipesAmountLeft = _maxSwipes;
            SwipesAmountChanged?.Invoke(_swipesAmountLeft);
        }

        public void DecreaseMaxSwipesAmount()
        {
            _maxSwipes--;
            MaxSwipesAmountDecreased?.Invoke(_maxSwipes);

            if (_maxSwipes > 0)
                return;

            if (_unitsAmountSavedAtThisSession > 0)
                OnGameWon();
            else
                OnGameLost();
        }

        public void ChangePausedState()
        {
            _isPaused = !_isPaused;
            _isInputActive = !_isInputActive;
            PauseStateChanged?.Invoke(_isPaused);
        }

        public void GoToMainMenu() => SceneManager.LoadScene("Scenes/MainMenu");

        public void IncreaseUnitsAmountSaved()
        {
            _unitsAmountSavedAtThisSession++;
            UnitsAmountSavedChanged?.Invoke(_totalUnitsAmountSaved + _unitsAmountSavedAtThisSession);
            DecreaseMaxSwipesAmount();
        }

        private void OnGameWon()
        {
            _difficultyManager.IncreaseDifficulty();
            _totalUnitsAmountSaved += _unitsAmountSavedAtThisSession;
            SaveData(_totalUnitsAmountSaved);
            _isInputActive = false;
            GameWon?.Invoke();
        }

        private void OnGameLost()
        {
            _isInputActive = false;
            GameLost?.Invoke();
        }

        public void RebuildLevel() => SceneManager.LoadScene("Scenes/SampleScene");

        public void GoToNextLevel()
        {
            RebuildLevel();
        }

        private void SaveData(int value)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path = Path.Combine(Application.persistentDataPath, "data.oi");
            FileStream stream = new FileStream(path, FileMode.Create);
            
            var data = new Data
            {
                totalUnitsAmountSaved = value,
                achievedDifficulty =  _difficultyManager.CurrentDifficulty
            };
            
            binaryFormatter.Serialize(stream, data);
            stream.Close();
        }

        private Data LoadData()
        {
            string path = Path.Combine(Application.persistentDataPath, "data.oi");
            
            if (File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                var data = binaryFormatter.Deserialize(stream) as Data;
                stream.Close();
                return data;
            }

            Debug.Log("Save file not found");
            return new Data()
            {
                totalUnitsAmountSaved = 0,
                achievedDifficulty = 1
            };
        }
    }
}