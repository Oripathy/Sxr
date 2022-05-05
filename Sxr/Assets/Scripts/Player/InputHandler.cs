using System;
using UnityEngine;

namespace Player
{
    internal class InputHandler : MonoBehaviour
    {
        public bool IsInputActive { get; set; }

        public event Action<Vector3> DirectionReceived;

        private void Update()
        {
            if (IsInputActive)
                Swipe();
        }

        private void Swipe()
        {
            DirectionReceived?.Invoke(Vector3.forward);
        }
    }
}