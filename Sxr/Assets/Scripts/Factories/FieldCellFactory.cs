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

        public void CreateFieldCell(Vector3 position) =>
            GameObject.Instantiate(_fieldCellPrefab, position, Quaternion.identity);

    }
}