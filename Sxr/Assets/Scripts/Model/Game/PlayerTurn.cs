using Player;

namespace Model.Game
{
    internal class PlayerTurn : GameState
    {
        private InputHandler _inputHandler;
        
        public PlayerTurn(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }
        
        public override void OnEnter()
        {
            _inputHandler.IsInputActive = false;
        }

        public override void OnExit()
        {
        }

        public override void SetNextState()
        {
            throw new System.NotImplementedException();
        }
    }
}