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
        private Touch _touch;
        private bool _isTouched;
        
        [field : SerializeField] public bool IsInputActive { get; set; }
        
        public event Action<Vector3> SwipeReceived;

        private void Update()
        {
            if (IsInputActive)
            {
                HandlePCInput(); 
            }
        }

        public void Init(Camera cameraMain) => _camera = cameraMain;

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
                    Debug.Log("sw");
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
        
        private void HandleMobileInput()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                if (_touch.phase == TouchPhase.Began)
                {
                    _isTouched = true;
                    _startPosition = _touch.position;
                }
                else if (_touch.phase == TouchPhase.Ended || _touch.phase == TouchPhase.Canceled)
                {
                    Touch(touch);
                    Reset();
                }
                
                DetectSwipe(touch);
            }
        }
        
        private void DetectSwipe(Touch touch)
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

        private void Touch(Touch touch)
        {
            var ray = _camera.ScreenPointToRay(touch.position);
                
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.TryGetComponent<ILockable>(out var lockable))
                    lockable.ChangeLockedState();
            }
        }

        private void Reset()
        {
            _startPosition = Vector2.zero;
            _isTouched = false;
        }
    }
}