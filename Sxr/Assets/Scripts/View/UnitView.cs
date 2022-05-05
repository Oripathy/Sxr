using System;
using System.Collections;
using Model;
using Model.GameField;
using Presenter;
using UnityEngine;
using View.Interfaces;

namespace View
{
    internal class UnitView : MonoBehaviour, IUnitView, ILockable
    {
        private UnitPresenter<IUnitView, UnitModel> _unitPresenter;
        private GameView _gameView;
        private Coroutine _coroutine;
        
        public event Action CollidedWithEnemy;

        private void Awake()
        {
            //_gameView.SwipeReceived += OnSwipeReceived;
        }

        public void Init(UnitPresenter<IUnitView, UnitModel> unitPresenter)
        {
            _unitPresenter = unitPresenter;
        }

        private void OnSwipeReceived(Vector3 direction) => _unitPresenter.Move(direction);
        
        public void Move(Vector3 position)
        {
            _coroutine = StartCoroutine(StartMoving(position));
        }

        private IEnumerator StartMoving(Vector3 position)
        {
            var startTime = Time.time;
            
            while (Time.time < startTime + 1f)
            {
                transform.position = Vector3.Slerp(transform.position, position, (Time.time - startTime) / 1f);
                yield return null;
            }

            transform.position = position;
        }

        public void ChangeLockedState() => _unitPresenter.ChangeLockedState();

        public void UpdateLockUI(bool isLocked)
        {
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            
        }
    }
}