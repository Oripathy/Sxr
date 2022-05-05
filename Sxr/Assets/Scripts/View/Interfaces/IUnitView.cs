using System;
using UnityEngine;

namespace View.Interfaces
{
    internal interface IUnitView : IBaseView
    {
        public event Action CollidedWithEnemy; 
        public void Move(Vector3 position);
        public void UpdateLockUI(bool isLocked);
    }
}