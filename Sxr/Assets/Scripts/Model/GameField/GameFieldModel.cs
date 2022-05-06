using System.Collections.Generic;
using DefaultNamespace;
using Factories;
using Presenter;
using UnityEngine;
using View;
using View.Interfaces;

namespace Model.GameField
{
    internal class GameFieldModel
    {
        private FieldCellFactory _fieldCellFactory;
        private int _rowsAmount = 10;
        private int _columnsAmount = 20;
        private Vector3 _initialSpawnPosition = new Vector3(0.5f, 0f, 0.5f);
        private Vector3 _cellSize = new Vector3(1f, 0f, 1f);
        private List<List<FieldCell>> _gameField = new List<List<FieldCell>>();

        public IReadOnlyList<IReadOnlyList<FieldCell>> GameField => _gameField.AsReadOnly();
        public int RowsAmount => _rowsAmount;
        public int ColumnsAmount => _columnsAmount;

        public GameFieldModel(FieldCellFactory fieldCellFactory)
        {
            _fieldCellFactory = fieldCellFactory;
        }

        public void Init()
        {
            var spawnPosition = _initialSpawnPosition;
            
            for (var row = 0; row < _rowsAmount; row++)
            {
                spawnPosition.x = _cellSize.x * row + _initialSpawnPosition.x;
                spawnPosition.z = _initialSpawnPosition.z;
                _gameField.Add(new List<FieldCell>());

                for (var column = 0; column < _columnsAmount; column++)
                {
                    spawnPosition.z = _cellSize.z * column + _initialSpawnPosition.z;
                    _gameField[row].Add(new FieldCell());
                    _gameField[row][column].CellPosition = spawnPosition;
                    _fieldCellFactory.CreateFieldCell(spawnPosition);
                }
            }
        }
        
        public bool IsCellEmpty(int row, int column, out Vector3 position)
        {
            position = _gameField[row][column].CellPosition;
            return _gameField[row][column].IsEmpty;
        }
    }
}