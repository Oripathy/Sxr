using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using View;

namespace Model.Game
{
    internal class GameModel
    {
        private UpdateHandler _updateHandler;
        private GameState _currentState;
        private Dictionary<Type, GameState> _statesByType;
        private bool _isInputActive;
        private bool _isPaused;
        private int _unitsAmountSaved;
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

        public int UnitsAmountSaved => _unitsAmountSaved;

        public event Action<Type> StateSwitched;
        public event Action<bool> InputActiveStateChanged;
        public event Action<int> SwipesAmountChanged;
        public event Action<int> MaxSwipesAmountDecreased;
        public event Action<int> UnitsAmountSavedChanged;
        public event Action<bool> PauseStateChanged;
        public event Action LevelRestarted;
        
        public GameModel()
        {
            _statesByType = new Dictionary<Type, GameState>
            {
                {typeof(PlayerTurn), new PlayerTurn(this)},
                {typeof(PlayerUnitsMoving), new PlayerUnitsMoving(this)},
                {typeof(EnemyTurn), new EnemyTurn(this)}
            };
        }

        public void Init(UpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += UpdatePass;
            IsInputActive = true;
            ResetSwipesAmountLeft();
            _currentState = _statesByType[typeof(PlayerTurn)];
            _currentState.OnEnter();
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
            SwipesAmountChanged?.Invoke(_maxSwipes);
            IsInputActive = true;
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
        }

        public void ChangePausedState()
        {
            _isPaused = !_isPaused;
            _isInputActive = !_isInputActive;
            PauseStateChanged?.Invoke(_isPaused);
        }

        public void IncreaseUnitsAmountSaved()
        {
            _unitsAmountSaved++;
            UnitsAmountSavedChanged?.Invoke(_unitsAmountSaved);
        }

        public void RebuildLevel() => SceneManager.LoadScene("Scenes/SampleScene");
    }
}