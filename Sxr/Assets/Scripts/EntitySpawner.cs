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
            var randomIndex = Random.Range(0, _cellsToSpawn.Count);
            var cellIndexes = _cellsToSpawn[randomIndex];
            _gameFieldModel.GameField[cellIndexes[0]][cellIndexes[1]].OccupiedBy = Entities.Unit;
            var cellPosition = _gameFieldModel.GameField[cellIndexes[0]][cellIndexes[1]].CellPosition +
                               new Vector3(0f, 0.5f, 0f);
            var unitModel = _unitFactory.CreateModel<UnitModel>(cellPosition, out var obj);
            unitModel.Init(_updateHandler);
            var unitView = obj.GetComponent<UnitView>();
            var unitPresenter =
                _presenterFactory.CreatePresenter<UnitPresenter, IUnitView, UnitModel>(unitView, unitModel,
                    _gameFieldPresenter);
            unitPresenter.ConcreteInit(_gamePresenter);
            unitView.Init(unitPresenter);
            unitModel.Position = cellPosition;
            unitModel.Row = cellIndexes[0];
            unitModel.Column = cellIndexes[1];
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

        for (var count = 0; count < difficulty; count++)
        {
            var randomIndex = Random.Range(0, _cellsToSpawn.Count);
            var cellIndexes = _cellsToSpawn[randomIndex];
            _gameFieldModel.GameField[cellIndexes[0]][cellIndexes[1]].OccupiedBy = Entities.Obstacle;
            var cellPosition = _gameFieldModel.GameField[cellIndexes[0]][cellIndexes[1]].CellPosition +
                               new Vector3(0f, 0.5f, 0f);
            var obstacle = _unitFactory.CreateModel<Obstacle>(cellPosition, out var obj);
            _cellsToSpawn.Remove(cellIndexes);
            var randomNumber = Random.Range(0f, 1f);

            if (randomNumber >= 0.5f)
            {
                randomIndex = Random.Range(0, _cellsToSpawn.Count);
                cellIndexes = _cellsToSpawn[randomIndex];
                _gameFieldModel.GameField[cellIndexes[0]][cellIndexes[1]].OccupiedBy = Entities.Enemy;
                cellPosition = _gameFieldModel.GameField[cellIndexes[0]][cellIndexes[1]].CellPosition +
                               new Vector3(0f, 0.5f, 0f);
                var enemyModel = _unitFactory.CreateModel<EnemyModel>(cellPosition, out obj);
                enemyModel.Init(_updateHandler);
                var enemyView = obj.GetComponent<EnemyView>();
                var enemyPresenter =
                    _presenterFactory.CreatePresenter<EnemyPresenter, IEnemyView, EnemyModel>(enemyView, enemyModel,
                        _gameFieldPresenter);
                enemyPresenter.ConcreteInit(_gamePresenter);
                enemyView.Init(enemyPresenter);
                enemyModel.Position = cellPosition;
                enemyModel.Row = cellIndexes[0];
                enemyModel.Column = cellIndexes[1];
            }
        }
        
        _cellsToSpawn.Clear();
    }
}