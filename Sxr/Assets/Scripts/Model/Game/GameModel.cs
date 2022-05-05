using Player;

namespace Model.Game
{
    internal class GameModel
    {
        private GameState _currentState;
        private InputHandler _inputHandler;
        private int _maxSwipes;
        private int _currentSwipesAmount;

        public PlayerTurn PlayerTurn { get; private set; }
        public PlayerUnitsMoving PlayerUnitsMoving { get; private set; }
        public EnemyTurn EnemyTurn { get; private set; }
    }
}