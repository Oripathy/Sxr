using System;
using UnityEngine;

namespace View.Interfaces
{
    internal interface IEnemyView : IBaseView
    {
        public event Action CollidedWithUnit;
        // public void UpdatePosition(Vector3 position);
        // public void DisableUnit();
    }
}