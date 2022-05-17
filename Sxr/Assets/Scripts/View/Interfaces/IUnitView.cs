using System;

namespace View.Interfaces
{
    internal interface IUnitView : IBaseView
    {
        public event Action CollidedWithEnemy;
        public event Action LockedStateChanged;
        public void UpdateLockUI(bool isLocked);
        public void UpdateRow(int row);
        public void UpdateColumn(int column);
    }
}