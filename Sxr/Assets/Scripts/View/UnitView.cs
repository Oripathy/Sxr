using System;
using Presenter;
using UnityEngine;
using View.Interfaces;

namespace View
{
    internal class UnitView : MonoBehaviour, IUnitView, ILockable
    {
        [SerializeField] private int _row;
        [SerializeField] private int _column;
        
        public event Action CollidedWithEnemy;
        public event Action LockedStateChanged;

        public void UpdatePosition(Vector3 position)
        {
            transform.position = position;
        }

        public void UpdateRow(int row) => _row = row + 1;

        public void UpdateColumn(int column) => _column = column + 1;

        public void UpdateLockUI(bool isLocked)
        {
            if (!isLocked)
                GetComponent<Renderer>().material.color = Color.green;
            else
                GetComponent<Renderer>().material.color = Color.gray;
        }

        public void DisableUnit() => gameObject.SetActive(false);

        public void EnableUnit() => gameObject.SetActive(true);

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnemyView>() != null)
                CollidedWithEnemy?.Invoke();
            
            Debug.Log(other);
        }

        public void ChangeLockedState()
        {
            LockedStateChanged?.Invoke();
        }
    }
}