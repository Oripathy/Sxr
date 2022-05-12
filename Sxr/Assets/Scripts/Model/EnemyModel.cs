using System;
using Model.GameField;
using UnityEngine;
using View;

namespace Model
{
    internal class EnemyModel : BaseModel
    {
        private UpdateHandler _updateHandler;
        private Entities _entity = Entities.Enemy;
        private Vector3 _position;
        private Vector3 _positionToMove;
        private Vector3 _direction;
        private float _startTime;
        private float _moveDuration = 0.5f;
        private bool _shouldMove;
        private int _row;
        private int _column;

        public Entities Entity => _entity;
        public Vector3 Position
        {
            get => _position;
            set
            {
                if (_position == value)
                    return;

                _position = value;
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
            }
        }

        public event Action<Vector3> PositionChanged;
        public event Action EnemyDestroyed;

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
            _direction = direction;
            _positionToMove = position;
            _startTime = Time.time;
            _shouldMove = true;
        }

        private void MoveUnit()
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
                _shouldMove = false;
                Row += (int) _direction.x;
                Column += (int) _direction.z;
            }
        }

        public void Destroy()
        {
            EnemyDestroyed?.Invoke();
            _updateHandler.UpdateTicked -= UpdatePass;
        }
    }
}