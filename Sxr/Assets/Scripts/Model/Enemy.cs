using UnityEngine;

namespace Model
{
    internal class Enemy : BaseModel
    {
        public Vector3 Position { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}