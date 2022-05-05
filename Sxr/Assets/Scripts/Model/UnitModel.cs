using System;
using Presenter;
using UnityEngine;
using View.Interfaces;

namespace Model
{
    internal class UnitModel : BaseModel
    {
        private UnitPresenter<IUnitView, UnitModel> _unitPresenter;
        private Vector3 _position;
        private int _row;
        private int _column;
        private bool _isLocked;
        
        public Vector3 Position
        {
            get => _position;
            set
            {
                if (_position == value) 
                    return;
                
                _position = value;
                PositionChanged?.Invoke(_position);
            }
        }

        public int Row
        {
            get => _row;
            set
            {
                if (_row == value)
                    return;

                _row = value;
                RowChanged?.Invoke(_row);
            }
        }

        public int Column
        {
            get => _column;
            set
            {
                if (_column == value)
                    return;

                _column = value;
                ColumnChanged?.Invoke(_column);
            }
        }

        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                if (value)
                    UnitLocked?.Invoke(true);
                else
                    UnitLocked?.Invoke(false);

                _isLocked = value;
            }
        }

        public event Action<Vector3> PositionChanged;
        public event Action<int> RowChanged;
        public event Action<int> ColumnChanged;
        public event Action<bool> UnitLocked;
    }
}