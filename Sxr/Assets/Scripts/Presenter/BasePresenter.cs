using Model;
using Model.GameField;
using UnityEngine;
using View.Interfaces;

namespace Presenter
{
    internal abstract class BasePresenter<K, U>
        where K : IBaseView
        where U : BaseModel
    {
        private protected K _view;
        private protected U _model;
        private protected GameFieldPresenter _gameFieldPresenter;
        private protected GamePresenter _gamePresenter;
        private protected bool _isSubscribed;

        public void Init(K view, U model, GameFieldPresenter gameFieldPresenter, GamePresenter gamePresenter)
        {
            _view = view;
            _model = model;
            _gameFieldPresenter = gameFieldPresenter;
            _gamePresenter = gamePresenter;
        }

        private protected abstract void Subscribe();
        private protected abstract void Unsubscribe();
        public abstract void OnPositionChanged(Vector3 position);
        public abstract void OnCollision();
        private protected abstract void OnLevelReloaded();
        public abstract void OnUnitDestroyed();
    }
}