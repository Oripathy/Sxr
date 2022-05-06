using System.Collections.Generic;
using DefaultNamespace;
using Factories;
using Model;
using Model.GameField;
using Presenter;
using UnityEngine;
using View;
using View.Interfaces;

internal class EntitySpawner
{
    private GameFieldModel _gameFieldModel;
    private DifficultyManager _difficultyManager;
    private UnitFactory _unitFactory;
    private PresenterFactory _presenterFactory;
    private List<int[]> _cellsToSpawn = new List<int[]>();
    private int _maxUnitsAmount = 5;
    private int[] _columnUnitSpawnBounds = {19, 20};

    public EntitySpawner(GameFieldModel gameFieldModel, DifficultyManager difficultyManager, UnitFactory unitFactory, PresenterFactory presenterFactory)
    {
        _gameFieldModel = gameFieldModel;
        _difficultyManager = difficultyManager;
        _unitFactory = unitFactory;
        _presenterFactory = presenterFactory;
    }

    public void SpawnUnits()
    {
        var rowsAmount = _gameFieldModel.RowsAmount;
        
        for (var row = 0; row < rowsAmount; row++)
        {
            _cellsToSpawn.Add(new []{row, _columnUnitSpawnBounds[0] - 1});
            _cellsToSpawn.Add(new []{row, _columnUnitSpawnBounds[1] - 1});
        }

        for (var count = 0; count < _maxUnitsAmount; count++)
        {
            var randomIndex = Random.Range(0, _cellsToSpawn.Count);
            var cellIndexes = _cellsToSpawn[randomIndex];
            var cellPosition = _gameFieldModel.GameField[cellIndexes[0]][cellIndexes[1]].CellPosition +
                               new Vector3(0f, 0.5f, 0f);
            var unit = _unitFactory.CreateModel<UnitModel>(cellPosition, out var obj);
            var view = obj.GetComponent<UnitView>();
            var unitPresenter =
                _presenterFactory.CreatePresenter<UnitPresenter, IUnitView, UnitModel>(view, unit,
                    _gameFieldModel);
            view.Init(unitPresenter);
            unit.Position = cellPosition;
            unit.Row = cellIndexes[0];
            unit.Column = cellIndexes[1];
            _cellsToSpawn.Remove(cellIndexes);
        }
        
        _cellsToSpawn.Clear();
    }

    public void SpawnEnemiesAndObstacles()
    {
        var difficulty = _difficultyManager.CurrentDifficulty;
        var rowsAmount = _gameFieldModel.RowsAmount;
        
        for (var row = 0; row < rowsAmount; row++)
        {
            for (var column = 0; column < _columnUnitSpawnBounds[0] - 1; column++)
            {
                _cellsToSpawn.Add(new[] {row, column});
            }
        }
        
        Debug.Log(_cellsToSpawn.Count);
        
        for (var count = 0; count < difficulty; count++)
        {
            var randomIndex = Random.Range(0, _cellsToSpawn.Count);
            var cellIndexes = _cellsToSpawn[randomIndex];
            var cellPosition = _gameFieldModel.GameField[cellIndexes[0]][cellIndexes[1]].CellPosition +
                               new Vector3(0f, 0.5f, 0f);
            var obstacle = _unitFactory.CreateModel<Obstacle>(cellPosition, out var obj);
            _cellsToSpawn.Remove(cellIndexes);
            var randomNumber = Random.Range(0f, 1f);

            if (randomNumber >= 0.5f)
            {
                randomIndex = Random.Range(0, _cellsToSpawn.Count);
                cellIndexes = _cellsToSpawn[randomIndex];
                cellPosition = _gameFieldModel.GameField[cellIndexes[0]][cellIndexes[1]].CellPosition +
                               new Vector3(0f, 0.5f, 0f);
                var enemy = _unitFactory.CreateModel<EnemyModel>(cellPosition, out obj);
                var view = obj.GetComponent<EnemyView>();
                var presenter = _presenterFactory.CreatePresenter<EnemyPresenter, IEnemyView, EnemyModel>(view, enemy, _gameFieldModel);
                view.Init(presenter);
                enemy.Position = cellPosition;
                enemy.Row = cellIndexes[0];
                enemy.Column = cellIndexes[1];
            }
        }
        
        _cellsToSpawn.Clear();
    }
}