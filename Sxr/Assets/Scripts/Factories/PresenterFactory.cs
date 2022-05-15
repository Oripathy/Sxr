using Model;
using Presenter;
using View.Interfaces;

namespace Factories
{
    internal class PresenterFactory
    {
        public T CreatePresenter<T, K, U>(K baseView, U baseModel, GameFieldPresenter gameFieldPresenter,
            GamePresenter gamePresenter)
            where T : BasePresenter<K, U>, new()
            where K : IBaseView
            where U : BaseModel
        {
            var presenter = new T();
            presenter.Init(baseView, baseModel, gameFieldPresenter, gamePresenter);
            return presenter;
        }
    }
}