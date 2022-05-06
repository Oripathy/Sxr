using Model;
using Model.GameField;
using UnityEngine;
using View.Interfaces;

namespace Presenter
{
    internal class UnitPresenter : BasePresenter<IUnitView, UnitModel>
    {
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