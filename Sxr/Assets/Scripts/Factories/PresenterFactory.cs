using Model;
using Model.GameField;
using Presenter;
using View.Interfaces;

namespace Factories
{
    internal class PresenterFactory
    {
        public T CreatePresenter<T, K, U>(K baseView, U baseModel, GameFieldModel gameFieldModel) 
            where T : BasePresenter<K, U>, new()
            where K : IBaseView
            where U : BaseModel
        {
            var presenter = new T();
            presenter.Init(baseView, baseModel, gameFieldModel);
            return new T();
        }
    }
}