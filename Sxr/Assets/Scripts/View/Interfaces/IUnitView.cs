using System;
using UnityEngine;

namespace View.Interfaces
{
    internal interface IUnitView : IBaseView
    {
        public event Action CollidedWithEnemy;
        public event Action LockedStateChanged;
        public void UpdateLockUI(bool isLocked);
        public void UpdatePosition(Vector3 position);
        public void UpdateRow(int row);
        public void UpdateColumn(int column);
        public void DestroyUnit();
    }
}