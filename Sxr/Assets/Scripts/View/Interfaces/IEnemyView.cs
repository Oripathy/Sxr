using System;
using UnityEngine;

namespace View.Interfaces
{
    internal interface IEnemyView : IBaseView
    {
        public event Action CollidedWithUnit;
    }
}