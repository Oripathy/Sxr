using System;
using UnityEngine;
using View.Interfaces;

namespace View
{
    internal class GameView : IGameView
    {
        public bool IsInputActive { get; set; }
        
        public event Action<Vector3> SwipeReceived;
        public event Action<Vector3> TouchReceived;

        private void Update()
        {
            if (IsInputActive)
            {
                Swipe();
                Touch();
            }
        }

        private void Swipe()
        {
            Vector3 direction = Vector3.zero;
            SwipeReceived?.Invoke(direction);
        }

        private void Touch()
        {
            Vector3 position = Vector3.zero;
            TouchReceived?.Invoke(position);
        }
    }
}