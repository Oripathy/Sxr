using System;
using System.Collections.Generic;
using DefaultNamespace;
using Factories;
using Model;
using Model.GameField;
using UnityEngine;

internal  class GameInstaller : MonoBehaviour
{
    [SerializeField] private GameObject _fieldCellPrefab;
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private DifficultyManager _difficultyManager;

    private FieldCellFactory _fieldCellFactory;
    private UnitFactory _unitFactory;
    private PresenterFactory _presenterFactory;
    private GameFieldModel _gameFieldModel;

    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        var prefabs = new Dictionary<Type, GameObject>
        {
            {typeof(UnitModel), _unitPrefab},
            {typeof(EnemyModel), _enemyPrefab},
            {typeof(Obstacle), _obstaclePrefab}
        };

        _fieldCellFactory = new FieldCellFactory(_fieldCellPrefab);
        _unitFactory = new UnitFactory(prefabs);
        _presenterFactory = new PresenterFactory();
        _gameFieldModel = new GameFieldModel(_fieldCellFactory);
        _gameFieldModel.Init();

        var entitySpawner = new EntitySpawner(_gameFieldModel, _difficultyManager, _unitFactory, _presenterFactory);
        entitySpawner.SpawnUnits();
        entitySpawner.SpawnEnemiesAndObstacles();
    }
}