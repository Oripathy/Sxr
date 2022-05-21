using UnityEngine;
using View;

namespace Model.GameField
{
    public enum Entities
    {
        Unit,
        Enemy,
        Obstacle,
        Nothing
    }
    
    internal class FieldCell
    {
        public FieldCellView _view; // for debug only

        public FieldCell(FieldCellView view)
        {
            _view = view;
        }        
        
        private Entities _occupiedBy;

        public Vector3 Position { get; set; }
        public Entities OccupiedBy
        {
            get => _occupiedBy;
            set
            {
                _occupiedBy = value; 
                _view.occupiedBy = value; // for debug only
            }
        }
    }
}