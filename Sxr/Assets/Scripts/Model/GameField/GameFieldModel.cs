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
        private UnitFactory _unitFactory;
        private PresenterFactory _presenterFactory;
        private DifficultyManager _difficultyManager;
        private List<int[]> _cellsToSpawn = new List<int[]>();
        private int _rowsAmount = 10;
        private int _columnsAmount = 20;
        private int _maxUnitsAmount = 5;
        private int[] _columnUnitSpawnBounds = {19, 20};
        private int[] _columnObstacleSpawnBounds = {1, 18};
        private Vector3 _initialSpawnPosition = new Vector3(0.5f, 0f, 0.5f);
        private Vector3 _cellSize = new Vector3(1f, 0f, 1f);
        private List<List<FieldCell>> _gameField = new List<List<FieldCell>>();

        public GameFieldModel(FieldCellFactory fieldCellFactory, UnitFactory unitFactory, PresenterFactory presenterFactory,
            DifficultyManager difficultyManager)
        {
            _fieldCellFactory = fieldCellFactory;
            _unitFactory = unitFactory;
            _presenterFactory = presenterFactory;
            _difficultyManager = difficultyManager;
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
            
            SpawnUnits();
            SpawnObstaclesAndEnemies();
        }

        private void SpawnUnits()
        {
            for (var row = 0; row < _rowsAmount; row++)
            {
                _cellsToSpawn.Add(new []{row, _columnUnitSpawnBounds[0] - 1});
                _cellsToSpawn.Add(new []{row, _columnUnitSpawnBounds[1] - 1});
            }

            for (var index = 0; index < _maxUnitsAmount; index++)
            {
                var randomIndex = Random.Range(0, _cellsToSpawn.Count);
                var cellIndexes = _cellsToSpawn[randomIndex];
                var cellPosition = _gameField[cellIndexes[0]][cellIndexes[1]].CellPosition + new Vector3(0f, 0.5f, 0f);
                var unit = _unitFactory.CreateModel<UnitModel>(cellPosition, out var obj);
                IUnitView view = obj.GetComponent<UnitView>();

                var unitPresenter =
                    _presenterFactory.CreatePresenter<UnitPresenter<IUnitView, UnitModel>, IUnitView, UnitModel>(view,
                        unit, this);
                obj.GetComponent<UnitView>().Init(unitPresenter);
                
                unit.Position = cellPosition;
                unit.Row = cellIndexes[0];
                unit.Column = cellIndexes[1];
                _cellsToSpawn.Remove(cellIndexes);
            }
            
            _cellsToSpawn.Clear();
        }

        private void SpawnObstaclesAndEnemies()
        {
            var difficulty = _difficultyManager.CurrentDifficulty;
            
            for (var row = 0; row < _rowsAmount; row++)
            {
                for (var column = _columnObstacleSpawnBounds[0] - 1;
                     column < _columnObstacleSpawnBounds[1] - 1;
                     column++)
                {
                    _cellsToSpawn.Add(new[] {row, column});
                }
            }

            for (var index = 0; index < difficulty; index++)
            {
                var randomIndex = Random.Range(0, _cellsToSpawn.Count);
                var cellIndexes = _cellsToSpawn[randomIndex];
                var cellPosition = _gameField[cellIndexes[0]][cellIndexes[1]].CellPosition + new Vector3(0f, 0.5f, 0f);
                var obstacle = _unitFactory.CreateModel<Obstacle>(cellPosition, out var obj);
                _cellsToSpawn.Remove(cellIndexes);
                var randomNumber = Random.Range(0f, 1f);

                if (randomNumber >= 0.5f)
                {
                    randomIndex = Random.Range(0, _cellsToSpawn.Count);
                    cellIndexes = _cellsToSpawn[randomIndex];
                    cellPosition = _gameField[cellIndexes[0]][cellIndexes[1]].CellPosition + new Vector3(0f, 0.5f, 0f);
                    var unit = _unitFactory.CreateModel<Enemy>(cellPosition, out obj);
                    unit.Position = cellPosition;
                    unit.Row = cellIndexes[0];
                    unit.Column = cellIndexes[1];
                }
            }
            
            _cellsToSpawn.Clear();
        }
        
        public bool IsCellEmpty(int row, int column, out Vector3 position)
        {
            position = _gameField[row][column].CellPosition;
            return _gameField[row][column].IsEmpty;
        }
    }
}