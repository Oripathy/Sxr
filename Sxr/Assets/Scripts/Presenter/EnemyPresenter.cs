using System.Collections.Generic;
using Model;
using UnityEngine;
using View.Interfaces;

namespace Presenter
{
    internal class EnemyPresenter : BasePresenter<IEnemyView, EnemyModel>
    {
        private GamePresenter _gamePresenter;
        private Dictionary<int, Vector3> _numbersToDirection;

        public void ConcreteInit(GamePresenter gamePresenter)
        {
            _gamePresenter = gamePresenter;
            _gamePresenter.EnemyTurnStarted += Move;
            _view.CollidedWithUnit += OnCollision;
            _model.PositionChanged += OnPositionChanged;
            _model.EnemyDestroyed += OnDestroyed;
            
            _numbersToDirection = new Dictionary<int, Vector3>
            {
                {0, Vector3.forward},
                {1, Vector3.back},
                {2, Vector3.left},
                {3, Vector3.right}
            };
        }

        private void Move()
        {
            var randomNumber = Random.Range(0, _numbersToDirection.Count);
            var direction = _numbersToDirection[randomNumber];
            
            if (_gameFieldPresenter.IsCellEmpty(_model.Row, _model.Column, direction, _model.Entity, out var position))
                _model.SetPositionToMove(position, direction);
        }

        private void OnPositionChanged(Vector3 position) => _view.UpdatePosition(position);

        private void OnCollision() => _model.Destroy();

        private void OnDestroyed()
        {
            _view.DestroyEnemy();
            _gameFieldPresenter.OnUnitDestroy(_model.Row, _model.Column);
        }
    }
}