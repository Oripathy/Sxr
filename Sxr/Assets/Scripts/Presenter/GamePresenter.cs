using System;
using Model.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using View.Interfaces;

namespace Presenter
{
    internal class GamePresenter
    {
        private IGameView _view;
        private GameModel _model;

        public event Action<Vector3> SwipeReceived;
        public event Action EnemyTurnStarted;
        public event Action LevelRestarted;
        
        public GamePresenter(IGameView view, GameModel model)
        {
            _view = view;
            _model = model;
        }

        public void Init()
        {
            _model.InputActiveStateChanged += UpdateInputActiveState;
            _view.SwipeReceived += OnSwipeReceived;
            _model.StateSwitched += OnStateChanged;
            _model.LevelRestarted += OnLevelRestarted;
        }

        private void OnSwipeReceived(Vector3 direction)
        {
            _model.IsSwipeReceived = true;
            SwipeReceived?.Invoke(direction);
        }

        private void OnStateChanged(Type type)
        {
            if (type == typeof(EnemyTurn))
                EnemyTurnStarted?.Invoke();
        }

        private void UpdateInputActiveState(bool isActive) => _view.IsInputActive = isActive;
        
        private void OnLevelRestarted() => LevelRestarted?.Invoke();
        
        public void OnUnitDestroy() => _model.DecreaseMaxSwipesAmount();
    }
}