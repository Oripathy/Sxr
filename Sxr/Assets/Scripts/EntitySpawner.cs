using System.Collections.Generic;
using Factories;
using Model;
using Model.GameField;
using Presenter;
using UnityEngine;
using View;
using View.Interfaces;

internal class EntitySpawner
{
    private GamePresenter _gamePresenter;
    private GameFieldPresenter _gameFieldPresenter;
    private GameFieldModel _gameFieldModel;
    private UpdateHandler _updateHandler;
    private DifficultyManager _difficultyManager;
    private UnitFactory _unitFactory;
    private PresenterFactory _presenterFactory;
    private List<int[]> _cellsToSpawn = new List<int[]>();
    private int _maxUnitsAmount = 5;
    private int _initialObstacleAmount = 10;
    private int[] _columnUnitSpawnBounds = {19, 20};

    public EntitySpawner(GamePresenter gamePresenter, GameFieldModel gameFieldModel, GameFieldPresenter gameFieldPresenter,
        DifficultyManager difficultyManager, UnitFactory unitFactory, PresenterFactory presenterFactory,
        UpdateHandler updateHandler)
    {
        _gamePresenter = gamePresenter;
        _gameFieldModel = gameFieldModel;
        _gameFieldPresenter = gameFieldPresenter;
        _difficultyManager = difficultyManager;
        _unitFactory = unitFactory;
        _presenterFactory = presenterFactory;
        _updateHandler = updateHandler;
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
            Spawn<UnitModel, IUnitView, UnitPresenter>(_cellsToSpawn, Entities.Unit);
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

        for (var count = 0; count < difficulty + _initialObstacleAmount; count++)
        {
            var randomIndex = Random.Range(0, _cellsToSpawn.Count);
            var cellIndexes = _cellsToSpawn[randomIndex];
            var cell = _gameFieldModel.GameField[cellIndexes[0]][cellIndexes[1]];
            cell.OccupiedBy = Entities.Obstacle;
            var obstacle = _unitFactory.CreateModel<Obstacle>(cell.Position + new Vector3(0f, 0.5f, 0f), out var obj);
            _cellsToSpawn.Remove(cellIndexes);
            var randomNumber = Random.Range(0f, 1f);

            if (randomNumber >= 0.5f)
                Spawn<EnemyModel, IEnemyView, EnemyPresenter>(_cellsToSpawn, Entities.Enemy);
        }
        
        _cellsToSpawn.Clear();
    }

    private void Spawn<T, K, U>(List<int[]> cellsToSpawn, Entities entity)
        where T : BaseModel, new()
        where K : IBaseView
        where U : BasePresenter<K, T>, new()
    {
        int randomIndex = Random.Range(0, cellsToSpawn.Count);
        var cellIndexes = cellsToSpawn[randomIndex];
        var cell = _gameFieldModel.GameField[cellIndexes[0]][cellIndexes[1]];
        cell.OccupiedBy = entity;
        var unitModel = _unitFactory.CreateModel<T>(cell.Position, out var obj);
        var unitView = obj.GetComponent<K>();
        _presenterFactory.CreatePresenter<U, K, T>(unitView, unitModel, _gameFieldPresenter, _gamePresenter);
        unitModel.Init(_updateHandler, cell.Position, cellIndexes[0], cellIndexes[1]);
        cellsToSpawn.Remove(cellIndexes);
    }
}