
namespace Model.Game
{
    internal class PlayerTurn : GameState
    {
        public PlayerTurn(GameModel model) : base(model)
        {
        }
        
        public override void OnEnter()
        {
            _model.IsInputActive = true;
            _model.IsSwipeReceived = false;
        }

        public override void UpdatePass()
        {
            SetNextState();    
        }
        
        public override void OnExit()
        {
            _model.IsInputActive = false;
        }

        public override void SetNextState()
        {
            if (!_model.IsSwipeReceived) 
                return;
            
            _model.SwitchState<PlayerUnitsMoving>();
        }
    }
}