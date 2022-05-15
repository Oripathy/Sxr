using UnityEngine;
using View;

namespace Model
{
    internal class Obstacle : BaseModel
    {
        public override void Init(UpdateHandler updateHandler, Vector3 position, int row, int column)
        {
            Position = position;
            Row = row;
            Column = column;
        }

        public override void SetPositionToMove(Vector3 position, Vector3 direction)
        {
        }

        private protected override void MoveUnit()
        {
        }

        public override void OnLevelReload()
        {
        }

        public override void Destroy()
        {
        }
    }
}