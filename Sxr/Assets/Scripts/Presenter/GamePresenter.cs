using Model.Game;
using UnityEngine;
using View.Interfaces;

namespace Presenter
{
    internal class GamePresenter
    {
        private IGameView _gameView;
        private GameModel _gameModel;

        public GamePresenter(IGameView gameView, GameModel gameModel)
        {
            _gameView = gameView;
            _gameModel = gameModel;
        }

        public void Init()
        {
            _gameView.SwipeReceived += OnSwipeReceived;
            _gameView.TouchReceived += OnTouchReceived;
        }

        private void OnSwipeReceived(Vector3 direction)
        {
            
        }

        private void OnTouchReceived(Vector3 position)
        {
            
        }
    }
}