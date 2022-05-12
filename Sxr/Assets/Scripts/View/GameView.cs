using System;
using UnityEngine;
using View.Interfaces;

namespace View
{
    internal class GameView : MonoBehaviour, IGameView
    {
        [SerializeField] private Camera _camera;
        [field : SerializeField] public bool IsInputActive { get; set; }
        
        public event Action<Vector3> SwipeReceived;
        public event Action<IUnitView> TouchReceived;

        private void Update()
        {
            if (IsInputActive)
            {
                Swipe();
                Touch();
            }
        }

        public void Init(Camera cameraMain) => _camera = cameraMain;

        private void Swipe()
        {
            if (Input.GetKeyDown(KeyCode.A))
                SwipeReceived?.Invoke(Vector3.back);
            else if (Input.GetKeyDown(KeyCode.D))
                SwipeReceived?.Invoke(Vector3.forward);
            else if (Input.GetKeyDown(KeyCode.W))
                SwipeReceived?.Invoke(Vector3.left);
            else if (Input.GetKeyDown(KeyCode.S))
                SwipeReceived?.Invoke(Vector3.right);
        }

        private void Touch()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.TryGetComponent<ILockable>(out var lockable))
                        lockable.ChangeLockedState();
                }
            }
        }
    }
}