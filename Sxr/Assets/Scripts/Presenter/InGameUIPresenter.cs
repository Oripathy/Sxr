using System;
using System.Collections.Generic;
using Model.Game;
using View.Interfaces;

namespace Presenter
{
    internal class InGameUIPresenter
    {
        private IInGameUI _view;
        private GameModel _gameModel;

        private Dictionary<Type, string> _textToType = new Dictionary<Type, string>
        {
            {typeof(EnemyTurn), "Enemy turn"},
            {typeof(PlayerTurn), "Your turn"}
        };

        public void Init(IInGameUI view, GameModel gameModel)
        {
            _view = view;
            _gameModel = gameModel;
            _view.SetInGameMenuActive(false);
            _view.PauseButtonPressed += OnPauseButtonPressed;
            _view.RestartButtonPressed += OnRestartButtonPressed;
            _view.RebuildButtonPressed += OnRebuildButtonPressed;
            _view.MainMenuButtonPressed += OnMainMenuButtonPressed;
            _view.ResumeButtonPressed += OnResumeButtonPressed;
            _view.NextLevelButtonPressed += OnNextLevelButtonPressed;
            
            _gameModel.StateSwitched += OnStateChanged;
            _gameModel.MaxSwipesAmountDecreased += OnMaxSwipesAmountChanged;
            _gameModel.UnitsAmountSavedChanged += OnUnitsAmountSavedChanged;
            _gameModel.SwipesAmountChanged += OnSwipesAmountLeftChanged;
            _gameModel.PauseStateChanged += ChangePauseMenuActiveState;
            _gameModel.GameWon += OnGameWon;
            _gameModel.GameLost += OnGameLost;
        }

        private void OnPauseButtonPressed() => _gameModel.ChangePausedState();
        private void OnRestartButtonPressed() => _gameModel.RestartLevel();
        private void OnRebuildButtonPressed() => _gameModel.RebuildLevel();
        private void OnMainMenuButtonPressed() => _gameModel.GoToMainMenu();
        private void OnResumeButtonPressed() => _gameModel.ChangePausedState();
        private void OnNextLevelButtonPressed() => _gameModel.GoToNextLevel();

        private void OnStateChanged(Type type)
        {
            if (_textToType.TryGetValue(type ,out var text))
                _view.UpdateTurnText(text);
        }

        private void OnMaxSwipesAmountChanged(int amount) => _view.UpdateUnitsAmountText(amount);

        private void OnUnitsAmountSavedChanged(int amount) => _view.UpdateUnitsAmountSavedText(amount);

        private void OnSwipesAmountLeftChanged(int amount) => _view.UpdateSwipesAmountLeftText(amount);

        private void ChangePauseMenuActiveState(bool isActive) => _view.SetInGameMenuActive(isActive);

        private void OnGameWon() => _view.ConvertMenuToWonMenu();

        private void OnGameLost() => _view.ConvertMenuToLostMenu();
    }
}