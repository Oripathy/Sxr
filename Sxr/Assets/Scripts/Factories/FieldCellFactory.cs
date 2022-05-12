using UnityEngine;
using View;

namespace Factories
{
    internal class FieldCellFactory
    {
        private GameObject _fieldCellPrefab;

        public FieldCellFactory(GameObject fieldCellPrefab)
        {
            _fieldCellPrefab = fieldCellPrefab;
        }

        public GameObject CreateFieldCell(Vector3 position)
        {
            return GameObject.Instantiate(_fieldCellPrefab, position, Quaternion.identity);
        }

    }
}