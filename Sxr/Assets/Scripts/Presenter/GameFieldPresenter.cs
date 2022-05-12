using Model.GameField;
using UnityEngine;

namespace Presenter
{
    internal class GameFieldPresenter
    {
        private GameFieldModel _model;

        public GameFieldPresenter(GameFieldModel model)
        {
            _model = model;
        }

        public void OnUnitDestroy(int row, int column)
        {
            _model.GameField[row][column].OccupiedBy = Entities.Nothing;
        }

        public bool IsCellEmpty(int row, int column, Vector3 direction, Entities entity, out Vector3 position)
        {
            var nextRow = row + (int) direction.x;
            var nextColumn = column + (int) direction.z;
        
            if (nextRow > _model.RowsAmount - 1 || nextColumn > _model.ColumnsAmount - 1 || nextRow < 0 ||
                nextColumn < 0)
            {
                position = Vector3.up;
                return false;
            }
        
            var occupiedBy = _model.GameField[nextRow][nextColumn].OccupiedBy;
        
            switch (entity)
            {
                case Entities.Unit:
                    if (occupiedBy == Entities.Unit || occupiedBy == Entities.Obstacle)
                    {
                        position = Vector3.up;
                        return false;
                    }
                    else
                    { 
                        position = _model.GameField[nextRow][nextColumn].CellPosition;
                        _model.GameField[row][column].OccupiedBy = Entities.Nothing;
                        
                        if (occupiedBy == Entities.Nothing)
                            _model.GameField[nextRow][nextColumn].OccupiedBy = entity;

                        return true;
                    }
                
                case Entities.Enemy:
                    if (occupiedBy == Entities.Enemy || occupiedBy == Entities.Obstacle)
                    {
                        position = Vector3.up;
                        return false;
                    }
                    else
                    {
                        position = _model.GameField[nextRow][nextColumn].CellPosition;
                        _model.GameField[row][column].OccupiedBy = Entities.Nothing;

                        if (occupiedBy == Entities.Nothing)
                            _model.GameField[nextRow][nextColumn].OccupiedBy = entity;

                        return true;
                    }
                
                default:
                    position = Vector3.up;
                    return false;
            }
        }
    }
}