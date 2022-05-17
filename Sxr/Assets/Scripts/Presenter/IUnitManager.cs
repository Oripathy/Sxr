using System;
using UnityEngine;

namespace Presenter
{
    internal interface IUnitManager
    {
        public event Action<Vector3> SwipeReceived;
        public event Action EnemyTurnStarted;
        public event Action LevelRestarted;
        public void OnUnitDestroyed();
        public void OnUnitReachedEndOfGameField();
    }
}