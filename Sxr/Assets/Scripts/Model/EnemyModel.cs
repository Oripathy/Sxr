using System;
using Model.GameField;
using UnityEngine;
using View;

namespace Model
{
    internal class EnemyModel : BaseModel
    {
        public override Vector3 Position
        {
            get => _position;
            set
            {
                if (_position == value)
                    return;

                _position = new Vector3(value.x, 0.5F, value.z);
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
            }
        }

        public event Action<Vector3> PositionChanged;
        public event Action EnemyDestroyed;

        public override void Init(UpdateHandler updateHandler, Vector3 position, int row, int column)
        {
            Entity = Entities.Enemy;
            _moveDuration = 0.5f;
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += UpdatePass;
            _isSubscribed = true;
            Position = position;
            _initialPosition = _position;
            Row = row;
            _initialRow = _row;
            Column = column;
            _initialColumn = _column;
        }

        public override void SetPositionToMove(Vector3 position, Vector3 direction)
        {
            _direction = direction;
            _positionToMove = position;
            _startTime = Time.time;
            _shouldMove = true;
        }

        private protected override void MoveUnit()
        {
            if (!_shouldMove)
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
            _shouldMove = false;                  

            if (!_isSubscribed)
                _updateHandler.UpdateTicked += UpdatePass;
        }

        public override void Destroy()
        {
            _updateHandler.UpdateTicked -= UpdatePass;
            EnemyDestroyed?.Invoke();
            _isSubscribed = false;
        }
    }
}