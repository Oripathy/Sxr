using System;
using System.Collections.Generic;
using View;

namespace Model.Game
{
    internal class GameModel
    {
        private UpdateHandler _updateHandler;
        private GameState _currentState;
        private Dictionary<Type, GameState> _statesByType;
        private bool _isInputActive;
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

        private void UpdatePass(float deltaTime)
        {
            _currentState.UpdatePass();
        }
    }
}