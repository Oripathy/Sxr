using UnityEngine;

namespace View.Interfaces
{
    internal interface IBaseView
    {
        public void UpdatePosition(Vector3 position);
        public void DisableUnit();
        public void EnableUnit();
    }
}