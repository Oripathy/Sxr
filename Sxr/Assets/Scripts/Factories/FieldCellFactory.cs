using UnityEngine;
using View;

namespace Factories
{
    internal class FieldCellFactory
    {
        private GameObject _fieldCellPrefab;
        private GameObject _endOfGameFieldCellPrefab;

        public FieldCellFactory(GameObject fieldCellPrefab, GameObject endOfGameFieldCellPrefab)
        {
            _fieldCellPrefab = fieldCellPrefab;
            _endOfGameFieldCellPrefab = endOfGameFieldCellPrefab;
        }

        public GameObject CreateFieldCell(Vector3 position, bool isEndOfGameField)
        {
            if (isEndOfGameField)
                return GameObject.Instantiate(_endOfGameFieldCellPrefab, position, Quaternion.identity);
            
            return GameObject.Instantiate(_fieldCellPrefab, position, Quaternion.identity);
        }
    }
}