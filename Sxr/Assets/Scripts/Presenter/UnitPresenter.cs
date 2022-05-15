using System.Collections;
using Model;
using UnityEngine;
using View.Interfaces;

namespace Presenter
{
    internal class UnitPresenter : BasePresenter<IUnitView, UnitModel>
    {
        // private GamePresenter _gamePresenter;
        // private bool _isSubscribed;

        public void ConcreteInit(/*GamePresenter gamePresenter*/)
        {
            // _gamePresenter = gamePresenter;
            _gamePresenter.LevelRestarted += OnLevelReloaded;
            Subscribe();
        }

        private protected override void Subscribe()
        {
            _gamePresenter.SwipeReceived += OnSwipeReceived;
            _model.PositionChanged += OnPositionChanged;
            _model.RowChanged += OnRowChanged;
            _model.ColumnChanged += OnColumnChanged;
            _model.UnitDestroyed += OnUnitDestroyed;
            _model.EndOfGameFieldReached += OnEndOfGameFieldReached;
            _model.UnitLocked += OnModelLockedStateChanged;
            _view.LockedStateChanged += OnTouchReceived;
            _view.CollidedWithEnemy += OnCollision;
            _isSubscribed = true;
        }

        private protected override void Unsubscribe()
        {
            _gamePresenter.SwipeReceived -= OnSwipeReceived;
            _model.PositionChanged -= OnPositionChanged;
            _model.RowChanged -= OnRowChanged;
            _model.ColumnChanged -= OnColumnChanged;
            _model.UnitDestroyed -= OnUnitDestroyed;
            _model.EndOfGameFieldReached -= OnEndOfGameFieldReached;
            _model.UnitLocked -= OnModelLockedStateChanged;
            _view.LockedStateChanged -= OnTouchReceived;
            _view.CollidedWithEnemy -= OnCollision;
            _isSubscribed = false;
        }

        private void OnSwipeReceived(Vector3 direction) => _model.UpdateHandler.ExecuteCoroutine(CheckCell(direction));

        private void OnTouchReceived() => ChangeLockedState();

        private IEnumerator CheckCell(Vector3 direction)
        {
            var checkCounts = 0;
            
            if (_model.IsLocked)
                yield break;
            
            while (checkCounts < 5)
            {
                if (!_gameFieldPresenter.IsCellEmpty(_model.Row, _model.Column, direction, _model.Entity, out var position))
                {
                    checkCounts++;
                    yield return null;
                }
                else
                {
                    _model.SetPositionToMove(position, direction);
                    break;
                }
            }
        }

        public void ChangeLockedState() => _model.IsLocked = !_model.IsLocked;

        public void OnModelLockedStateChanged(bool isLocked) => _view.UpdateLockUI(isLocked);

        public override void OnPositionChanged(Vector3 position) => _view.UpdatePosition(position);

        public void OnRowChanged(int row) => _view.UpdateRow(row);
        
        public void OnColumnChanged(int column) => _view.UpdateColumn(column);
        
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

        public override void OnUnitDestroyed()
        {
            _view.DisableUnit();
            _gamePresenter.OnUnitDestroy();
            _gameFieldPresenter.ReleaseCell(_model.Row, _model.Column);
            Unsubscribe();
        }

        public void OnEndOfGameFieldReached()
        {
            
        }
    }
}