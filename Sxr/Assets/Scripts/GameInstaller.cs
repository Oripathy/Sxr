using System;
using System.Collections.Generic;
using Factories;
using Model;
using Model.Game;
using Model.GameField;
using Presenter;
using UnityEngine;
using View;

internal  class GameInstaller : MonoBehaviour
{
    [SerializeField] private GameObject _fieldCellPrefab;
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private DifficultyManager _difficultyManager;
    [SerializeField] private UpdateHandler _updateHandlerPrefab;
    [SerializeField] private GameView _gameViewPrefab;

    private FieldCellFactory _fieldCellFactory;
    private UnitFactory _unitFactory;
    private PresenterFactory _presenterFactory;
    private GameFieldModel _gameFieldModel;
    private GameFieldPresenter _gameFieldPresenter;
    private GameModel _gameModel;
    private GamePresenter _gamePresenter;
    private GameView _gameView;
    private UpdateHandler _updateHandler;

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

        _updateHandler = Instantiate(_updateHandlerPrefab).GetComponent<UpdateHandler>();
        _fieldCellFactory = new FieldCellFactory(_fieldCellPrefab);
        _unitFactory = new UnitFactory(prefabs);
        _presenterFactory = new PresenterFactory();
        _gameView = Instantiate(_gameViewPrefab).GetComponent<GameView>();
        _gameView.Init(_camera);
        _gameModel = new GameModel();
        _gamePresenter = new GamePresenter(_gameView, _gameModel);
        _gamePresenter.Init();
        _gameModel.Init(_updateHandler);
        _gameFieldModel = new GameFieldModel(_fieldCellFactory);
        _gameFieldModel.Init();
        _gameFieldPresenter = new GameFieldPresenter(_gameFieldModel);

        var entitySpawner = new EntitySpawner(_gamePresenter, _gameFieldModel, _gameFieldPresenter, _difficultyManager,
            _unitFactory, _presenterFactory, _updateHandler);
        entitySpawner.SpawnUnits();
        entitySpawner.SpawnEnemiesAndObstacles();
    }
}