using System.Collections.Generic;
using Model;
using UnityEngine;
using View.Interfaces;

namespace Presenter
{
    internal class EnemyPresenter : BasePresenter<IEnemyView, EnemyModel>
    {
        // private GamePresenter _gamePresenter;
        private Dictionary<int, Vector3> _numbersToDirection;

        public void ConcreteInit(/*GamePresenter gamePresenter*/)
        {
            _gamePresenter.LevelRestarted += OnLevelReloaded;
            // _gamePresenter = gamePresenter;
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
            _gamePresenter.EnemyTurnStarted += Move;
            _model.PositionChanged += OnPositionChanged;
            _isSubscribed = true;
        }

        private protected override void Unsubscribe()
        {
            _view.CollidedWithUnit -= OnCollision;
            _model.EnemyDestroyed -= OnUnitDestroyed;
            _gamePresenter.EnemyTurnStarted -= Move;
            _model.PositionChanged -= OnPositionChanged;
            _isSubscribed = false;
        }

        public override void OnUnitDestroyed()
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

        public override void OnPositionChanged(Vector3 position) => _view.UpdatePosition(position);

        public override void OnCollision() => _model.Destroy();

        private protected override void OnLevelReloaded()
        {
            _gameFieldPresenter.ReleaseCell(_model.Row, _model.Column);
            _view.EnableUnit();
            _model.OnLevelReload();
            _gameFieldPresenter.TakeCell(_model.Row, _model.Column, _model.Entity);
            
            if (!_isSubscribed)
                Subscribe();
        }
    }
}