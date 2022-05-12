using UnityEngine;

namespace Model.Game
{
    internal class EnemyTurn : GameState
    {
        private float _startTime;
        private float _turnDuration = 0.5f;
        
        public EnemyTurn(GameModel model) : base(model)
        {
        }
        
        public override void OnEnter()
        {
            _startTime = Time.time;
        }

        public override void UpdatePass()
        {
            SetNextState();
        }

        public override void OnExit()
        {
            _model.ResetSwipesAmountLeft();
        }

        public override void SetNextState()
        {
            if (Time.time < _startTime + _turnDuration)
                return;
            
            _model.SwitchState<PlayerTurn>();
        }
    }
}