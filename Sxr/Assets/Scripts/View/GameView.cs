using System;
using UnityEngine;
using View.Interfaces;

namespace View
{
    internal class GameView : MonoBehaviour, IGameView
    {
        [SerializeField] private Camera _camera;

        private Vector2 _startPosition;
        private Vector2 _swipeDelta;
        private bool _isTouched;
        
        [field : SerializeField] public bool IsInputActive { get; set; }
        
        public event Action<Vector3> SwipeReceived;

        private void Update()
        {
            if (IsInputActive)
            {
                HandlePCInput(); 
                //HandleMobileInput();
            }
        }

        public void Init(Camera cameraMain) => _camera = cameraMain;

        #region PCInput
        private void HandlePCInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isTouched = true;
                _startPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                PCTouch();
                Reset();
            }

            DetectPCSwipe();
        }
        
        private void DetectPCSwipe()
        {
            if (_isTouched)
            {
                _swipeDelta = (Vector2) Input.mousePosition - _startPosition;

                if (_swipeDelta.magnitude > 150)
                {
                    float x = _swipeDelta.x;
                    float y = _swipeDelta.y;
                    var direction = new Vector3();

                    if (Math.Abs(x) > Math.Abs(y))
                    {
                        direction = new Vector3(0f, 0f, Math.Sign(x) * 1f);
                    }
                    else
                    {
                        direction = new Vector3(Math.Sign(y) * -1f, 0f, 0f);
                    }

                    SwipeReceived?.Invoke(direction);
                    Reset();
                }
            }
        }

        private void PCTouch()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
                
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.TryGetComponent<ILockable>(out var lockable))
                    lockable.ChangeLockedState();
            }
        }
        #endregion

        #region MobileInput
        private void HandleMobileInput()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    _isTouched = true;
                    _startPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    Touch(ref touch);
                    Reset();
                }
                
                DetectSwipe(ref touch);
            }
        }
        
        private void DetectSwipe(ref Touch touch)
        {
            if (_isTouched)
            {
                _swipeDelta = touch.position - _startPosition;

                if (_swipeDelta.magnitude > 100)
                {
                    float x = _swipeDelta.x;
                    float y = _swipeDelta.y;
                    var direction = new Vector3();

                    if (Math.Abs(x) > Math.Abs(y))
                    {
                        direction = new Vector3(0f, 0f, Math.Sign(x) * 1f);
                    }
                    else
                    {
                        direction = new Vector3(Math.Sign(y) * -1f, 0f, 0f);
                    }

                    SwipeReceived?.Invoke(direction);
                    Reset();
                }
            }
        }

        private void Touch(ref Touch touch)
        {
            var ray = _camera.ScreenPointToRay(touch.position);
                
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.TryGetComponent<ILockable>(out var lockable))
                    lockable.ChangeLockedState();
            }
        }
        #endregion

        private void Reset()
        {
            _startPosition = Vector2.zero;
            _isTouched = false;
        }
    }
}