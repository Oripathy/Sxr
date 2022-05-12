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

        public void Init(K view, U model, GameFieldPresenter gameFieldPresenter)
        {
            _view = view;
            _model = model;
            _gameFieldPresenter = gameFieldPresenter;
        }
    }
}