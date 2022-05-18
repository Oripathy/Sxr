using System.Collections;
using Model;
using UnityEngine;
using View.Interfaces;

namespace Presenter
{
    internal class UnitPresenter : BasePresenter<IUnitView, UnitModel>
    {
        private protected override void ConcreteInit()
        {
            _unitManager.LevelRestarted += OnLevelReloaded;
            Subscribe();
        }

        private protected override void Subscribe()
        {
            _unitManager.SwipeReceived += OnSwipeReceived;
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
            _unitManager.SwipeReceived -= OnSwipeReceived;
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

        private void ChangeLockedState() => _model.IsLocked = !_model.IsLocked;

        private void OnModelLockedStateChanged(bool isLocked) => _view.UpdateLockUI(isLocked);

        private protected override void OnPositionChanged(Vector3 position) => _view.UpdatePosition(position);

        private void OnRowChanged(int row) => _view.UpdateRow(row);

        private void OnColumnChanged(int column) => _view.UpdateColumn(column);
        
        private protected override void OnCollision() => _model.Destroy();

        private protected override void OnLevelReloaded()
        {
            if (!_isSubscribed)
                Subscribe();
            
            _gameFieldPresenter.ReleaseCell(_model.Row, _model.Column);
            _model.OnLevelReload();
            _view.EnableUnit();
            _gameFieldPresenter.TakeCell(_model.Row, _model.Column, _model.Entity);
        }

        private protected override void OnUnitDestroyed()
        {
            _view.DisableUnit();
            _unitManager.OnUnitDestroyed();
            _gameFieldPresenter.ReleaseCell(_model.Row, _model.Column);
            Unsubscribe();
        }

        private void OnEndOfGameFieldReached()
        {
            _view.DisableUnit();
            _unitManager.OnUnitReachedEndOfGameField();
            _gameFieldPresenter.ReleaseCell(_model.Row, _model.Column);
            Unsubscribe();
        }
    }
}