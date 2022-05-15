using System;
using System.Collections.Generic;
using Model.Game;
using View.Interfaces;

namespace Presenter
{
    internal class InGameUIPresenter
    {
        //TODO : Connect this shit with GameModel for events
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
            _view.SetPauseMenuActive(false);
            _view.PauseButtonPressed += OnPauseButtonPressed;
            _view.RestartButtonPressed += OnRestartButtonPressed;
            _view.RebuildButtonPressed += OnRebuildButtonPressed;
            _view.MainMenuButtonPressed += OnMainMenuButtonPressed;
            _view.ResumeButtonPressed += OnResumeButtonPressed;
            
            _gameModel.StateSwitched += OnStateChanged;
            _gameModel.MaxSwipesAmountDecreased += OnMaxSwipesAmountChanged;
            _gameModel.UnitsAmountSavedChanged += OnUnitsAmountSavedChanged;
            _gameModel.SwipesAmountChanged += OnSwipesAmountLeftChanged;
            _gameModel.PauseStateChanged += ChangePauseMenuActiveState;
        }

        private void OnPauseButtonPressed() => _gameModel.ChangePausedState();
        private void OnRestartButtonPressed() => _gameModel.RestartLevel();
        private void OnRebuildButtonPressed() => _gameModel.RebuildLevel();
        private void OnMainMenuButtonPressed(){}
        private void OnResumeButtonPressed() => _gameModel.ChangePausedState();

        private void OnStateChanged(Type type)
        {
            if (_textToType.TryGetValue(type ,out var text))
                _view.UpdateTurnText(text);
        }

        private void OnMaxSwipesAmountChanged(int amount) => _view.UpdateUnitsAmountText(amount);

        private void OnUnitsAmountSavedChanged(int amount) => _view.UpdateUnitsAmountSavedText(amount);

        private void OnSwipesAmountLeftChanged(int amount) => _view.UpdateSwipesAmountLeftText(amount);

        public void ChangePauseMenuActiveState(bool isActive) => _view.SetPauseMenuActive(isActive);
    }
}