using System.Collections;
using Model;
using UnityEngine;
using View.Interfaces;

namespace Presenter
{
    internal class UnitPresenter : BasePresenter<IUnitView, UnitModel>
    {
        private GamePresenter _gamePresenter;
        
        public void ConcreteInit(GamePresenter gamePresenter)
        {
            _gamePresenter = gamePresenter;
            //_gamePresenter.SwipeReceived += Move;
            _gamePresenter.SwipeReceived += OnSwipeReceived;
            //_gamePresenter.TouchReceived += OnTouchReceived;
            _view.LockedStateChanged += OnTouchReceived;
            _model.PositionChanged += OnPositionChanged;
            _model.RowChanged += OnRowChanged;
            _model.ColumnChanged += OnColumnChanged;
            _model.UnitDestroyed += OnUnitDestroyed;
            _model.EndOfGameFieldReached += OnEndOfGameFieldReached;
            _model.UnitLocked += OnModelLockedStateChanged;
            _view.CollidedWithEnemy += OnCollision;
        }

        // private void Move(Vector3 direction)
        // {
        //     if (_gameFieldPresenter.IsCellEmpty(_model.Row, _model.Column, direction, out var position))
        //         _model.SetPositionToMove(position, direction);
        // }

        private void OnSwipeReceived(Vector3 direction) => _model.UpdateHandler.ExecuteCoroutine(CheckCell(direction));

        private void OnTouchReceived()
        {
            //if (unitView == _view)
                ChangeLockedState();
        }
        

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

        public void OnPositionChanged(Vector3 position)
        {
            _view.UpdatePosition(position);
        }

        public void OnRowChanged(int row) => _view.UpdateRow(row);
        
        public void OnColumnChanged(int column) => _view.UpdateColumn(column);
        
        public void OnCollision() => _model.Destroy();

        public void OnUnitDestroyed()
        {
            _view.DestroyUnit();
            _gamePresenter.OnUnitDestroy();
            _gameFieldPresenter.OnUnitDestroy(_model.Row, _model.Column);
        }

        public void OnEndOfGameFieldReached()
        {
            
        }
    }
}