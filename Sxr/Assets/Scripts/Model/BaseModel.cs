using Model.GameField;
using UnityEngine;
using View;

namespace Model
{
    internal abstract class BaseModel
    {
        private protected UpdateHandler _updateHandler;
        // private protected Entities _entity;
        private protected Vector3 _position;
        private protected Vector3 _initialPosition;
        private protected Vector3 _positionToMove;
        private protected Vector3 _direction;
        private protected float _startTime;
        private protected float _moveDuration;
        private protected int _row;
        private protected int _initialRow;
        private protected int _column;
        private protected int _initialColumn;
        private protected bool _shouldMove;
        private protected bool _isSubscribed;

        public Entities Entity { get; private protected set; }
        public virtual Vector3 Position { get; set; }
        public virtual int Row { get; set; }
        public virtual int Column { get; set; }

        public abstract void Init(UpdateHandler updateHandler, Vector3 position, int row, int column);
        private protected virtual void UpdatePass() => MoveUnit();
        public abstract void SetPositionToMove(Vector3 position, Vector3 direction);
        private protected abstract void MoveUnit();
        public abstract void OnLevelReload();
        public abstract void Destroy();
    }
}