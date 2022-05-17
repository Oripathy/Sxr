using System;
using Model.GameField;
using UnityEngine;
using View;

namespace Model
{
    internal class UnitModel : BaseModel
    {
        private bool _isLocked;

        public UpdateHandler UpdateHandler => _updateHandler;
        public override Vector3 Position
        {
            get => _position;
            set
            {
                if (_position == value) 
                    return;
                
                _position = new Vector3(value.x, 0.5f, value.z);
                PositionChanged?.Invoke(_position);
            }
        }

        public override int Row
        {
            get => _row;
            set
            {
                if (_row == value)
                    return;

                _row = value;
                RowChanged?.Invoke(_row);
            }
        }

        public override int Column
        {
            get => _column;
            set
            {
                if (_column == value)
                    return;

                _column = value;
                ColumnChanged?.Invoke(_column);
                
                if (_column == 0)
                {
                    EndOfGameFieldReached?.Invoke();
                    Unsubscribe();
                }
            }
        }

        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;
                UnitLocked?.Invoke(_isLocked);
            }
        }

        public event Action<Vector3> PositionChanged;
        public event Action<int> RowChanged;
        public event Action<int> ColumnChanged;
        public event Action<bool> UnitLocked;
        public event Action UnitDestroyed;
        public event Action EndOfGameFieldReached;


        public override void Init(UpdateHandler updateHandler, Vector3 position, int row, int column)
        {
            Entity = Entities.Unit;
            _moveDuration = 0.5f;
            _updateHandler = updateHandler;
            Subscribe();
            Position = position;
            _initialPosition = _position;
            Row = row;
            _initialRow = _row;
            Column = column;
            _initialColumn = _column;
        }

        public override void SetPositionToMove(Vector3 position, Vector3 direction)
        {
            _positionToMove = position;
            _startTime = Time.time;
            _direction = direction;
            _shouldMove = true;
        }
        
        private protected override void MoveUnit()
        {
            if (!_shouldMove || _isLocked) 
                return;

            if (Time.time < _startTime + _moveDuration)
            {
                Position = Vector3.Slerp(_position, _positionToMove, (Time.time - _startTime) / _moveDuration);
            }
            else
            {
                Position = _positionToMove;
                Row += (int) _direction.x;
                Column += (int) _direction.z;
                _shouldMove = false;
            }
        }

        public override void OnLevelReload()
        {
            Position = _initialPosition;
            Row = _initialRow;
            Column = _initialColumn;
            IsLocked = false;
            _shouldMove = false;

            if (!_isSubscribed)
                Subscribe();
        }

        public override void Destroy()
        {
            UnitDestroyed?.Invoke();
            Unsubscribe();
        }

        private void Subscribe()
        {
            _updateHandler.UpdateTicked += UpdatePass;
            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            _updateHandler.UpdateTicked -= UpdatePass;
            _isSubscribed = false;
        }
    }
}