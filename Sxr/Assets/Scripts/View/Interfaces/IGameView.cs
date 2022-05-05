using System;
using UnityEngine;

namespace View.Interfaces
{
    internal interface IGameView
    {
        public event Action<Vector3> SwipeReceived;
        public event Action<Vector3> TouchReceived;
    }
}