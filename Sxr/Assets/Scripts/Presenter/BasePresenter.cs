using Model;
using Model.GameField;
using View.Interfaces;

namespace Presenter
{
    internal abstract class BasePresenter<K, U>
        where K : IBaseView
        where U : BaseModel
    {
        private protected K _view;
        private protected U _model;
        private protected GameFieldModel _gameFieldModel;

        public void Init(K view, U model, GameFieldModel gameFieldModel)
        {
            _view = view;
            _model = model;
            _gameFieldModel = gameFieldModel;
        }
    }
}