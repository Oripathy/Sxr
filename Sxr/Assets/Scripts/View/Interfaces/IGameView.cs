using System;
using UnityEngine;

namespace View.Interfaces
{
    internal interface IGameView
    {
        public bool IsInputActive { get; set; }
        public event Action<Vector3> SwipeReceived;
        public event Action<IUnitView> TouchReceived;
    }
}