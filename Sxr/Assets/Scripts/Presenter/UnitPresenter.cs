using Model;
using Model.GameField;
using UnityEngine;
using View.Interfaces;

namespace Presenter
{
    internal class UnitPresenter<K, U> : BasePresenter<IUnitView, UnitModel>
        // where K : IUnitView
        // where U : UnitModel
    {
        // private IUnitView _unitView;
        // private UnitModel _unitModel;
        // private GameFieldModel _gameField;

        // public UnitPresenter(IUnitView unitView, Model.UnitModel unitModel, GameFieldModel gameField)
        // {
        //     _unitView = unitView;
        //     _unitModel = unitModel;
        //     _gameField = gameField;
        // }

        // public override void Init()
        // {
        //     _unitModel.UnitLocked += OnModelLockedStateChanged;
        // }

        public void Move(Vector3 direction)
        {
            if (_gameFieldModel.IsCellEmpty(_model.Row + (int) direction.x, _model.Column + (int) direction.z,
                    out var position))
                _view.Move(position);
        }

        public void ChangeLockedState() => _model.IsLocked = !_model.IsLocked;

        public void OnModelLockedStateChanged(bool isLocked) => _view.UpdateLockUI(isLocked);
    }
}