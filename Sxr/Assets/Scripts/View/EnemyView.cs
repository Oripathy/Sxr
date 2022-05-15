using System;
using Presenter;
using UnityEngine;
using View.Interfaces;

namespace View
{
    internal class EnemyView : MonoBehaviour, IEnemyView
    {
        private EnemyPresenter _enemyPresenter;

        public event Action CollidedWithUnit;
        
        public void Init(EnemyPresenter enemyPresenter)
        {
            _enemyPresenter = enemyPresenter;
        }

        public void UpdatePosition(Vector3 position) => transform.position = position;

        public void DisableUnit() => gameObject.SetActive(false);

        public void EnableUnit() => gameObject.SetActive(true);

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<UnitView>() != null)
                CollidedWithUnit?.Invoke();
        }
    }
}