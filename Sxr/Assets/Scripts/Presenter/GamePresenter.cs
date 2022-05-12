using System;
using System.Runtime.CompilerServices;
using Model.Game;
using UnityEngine;
using View.Interfaces;

namespace Presenter
{
    internal class GamePresenter
    {
        private IGameView _view;
        private GameModel _model;

        public event Action<Vector3> SwipeReceived;
        public event Action<IUnitView> TouchReceived;
        public event Action EnemyTurnStarted;
        
        public GamePresenter(IGameView view, GameModel model)
        {
            _view = view;
            _model = model;
        }

        public void Init()
        {
            _model.InputActiveStateChanged += UpdateInputActiveState;
            _model.StateSwitched += OnStateChanged;
            _view.SwipeReceived += OnSwipeReceived;
            _view.TouchReceived += OnTouchReceived;
        }

        private void OnSwipeReceived(Vector3 direction)
        {
            _model.IsSwipeReceived = true;
            SwipeReceived?.Invoke(direction);
        }

        private void OnTouchReceived(IUnitView unitView)
        {
            TouchReceived?.Invoke(unitView);
        }

        private void OnStateChanged(Type type)
        {
            if (type == typeof(EnemyTurn))
                EnemyTurnStarted?.Invoke();
        }

        private void UpdateInputActiveState(bool isActive) => _view.IsInputActive = isActive;
        public void OnUnitDestroy() => _model.DecreaseMaxSwipesAmount();
    }
}