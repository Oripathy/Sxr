using System;
using Model.GameField;
using UnityEngine;
using View;

namespace Model
{
    internal class UnitModel : BaseModel
    {
        private UpdateHandler _updateHandler;
        private Entities _entity = Entities.Unit;
        private Vector3 _position;
        private Vector3 _positionToMove;
        private Vector3 _direction;
        private float _startTime;
        private float _moveDuration = 0.5f;
        private int _row;
        private int _column;
        private bool _isLocked;
        private bool _shouldMove;

        public Entities Entity => _entity;
        public UpdateHandler UpdateHandler => _updateHandler;
        public Vector3 Position
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

        public int Row
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

        public int Column
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
                    Destroy();
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

        public void Init(UpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;
            _updateHandler.UpdateTicked += UpdatePass;
        }

        private void UpdatePass(float deltaTime)
        {
            MoveUnit();
        }
        
        public void SetPositionToMove(Vector3 position, Vector3 direction)
        {
            _positionToMove = position;
            _startTime = Time.time;
            _direction = direction;
            _shouldMove = true;
        }
        
        private void MoveUnit()
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

        public void Destroy()
        {
            UnitDestroyed?.Invoke();
            _updateHandler.UpdateTicked -= UpdatePass;
        }
    }
}