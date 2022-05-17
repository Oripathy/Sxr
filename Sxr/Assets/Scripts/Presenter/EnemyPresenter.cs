using System.Collections.Generic;
using Model;
using UnityEngine;
using View.Interfaces;

namespace Presenter
{
    internal class EnemyPresenter : BasePresenter<IEnemyView, EnemyModel>
    {
        private Dictionary<int, Vector3> _numbersToDirection;

        public void ConcreteInit()
        {
            _unitManager.LevelRestarted += OnLevelReloaded;
            _numbersToDirection = new Dictionary<int, Vector3>
            {
                {0, Vector3.forward},
                {1, Vector3.back},
                {2, Vector3.left},
                {3, Vector3.right}
            };
            
            Subscribe();
        }

        private protected override void Subscribe()
        {
            _view.CollidedWithUnit += OnCollision;
            _model.EnemyDestroyed += OnUnitDestroyed;
            _unitManager.EnemyTurnStarted += Move;
            _model.PositionChanged += OnPositionChanged;
            _isSubscribed = true;
        }

        private protected override void Unsubscribe()
        {
            _view.CollidedWithUnit -= OnCollision;
            _model.EnemyDestroyed -= OnUnitDestroyed;
            _unitManager.EnemyTurnStarted -= Move;
            _model.PositionChanged -= OnPositionChanged;
            _isSubscribed = false;
        }

        private protected override void OnUnitDestroyed()
        {
            _view.DisableUnit();
            _gameFieldPresenter.ReleaseCell(_model.Row, _model.Column);
            Unsubscribe();
        }
        
        private void Move()
        {
            var randomNumber = Random.Range(0, _numbersToDirection.Count);
            var direction = _numbersToDirection[randomNumber];
            
            if (_gameFieldPresenter.IsCellEmpty(_model.Row, _model.Column, direction, _model.Entity, out var position))
                _model.SetPositionToMove(position, direction);
        }

        private protected override void OnPositionChanged(Vector3 position) => _view.UpdatePosition(position);

        private protected override void OnCollision() => _model.Destroy();

        private protected override void OnLevelReloaded()
        {
            if (!_isSubscribed)
                Subscribe();
            
            _model.OnLevelReload();
            _view.EnableUnit();
            _gameFieldPresenter.ReleaseCell(_model.Row, _model.Column);
            _gameFieldPresenter.TakeCell(_model.Row, _model.Column, _model.Entity);
        }
    }
}