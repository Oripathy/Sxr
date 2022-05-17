using Model;
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
        private protected IUnitManager _unitManager;
        private protected bool _isSubscribed;

        public void Init(K view, U model, GameFieldPresenter gameFieldPresenter, IUnitManager unitManager)
        {
            _view = view;
            _model = model;
            _gameFieldPresenter = gameFieldPresenter;
            _unitManager = unitManager;
        }

        private protected abstract void Subscribe();
        private protected abstract void Unsubscribe();
        private protected abstract void OnPositionChanged(Vector3 position);
        private protected abstract void OnCollision();
        private protected abstract void OnLevelReloaded();
        private protected abstract void OnUnitDestroyed();
    }
}