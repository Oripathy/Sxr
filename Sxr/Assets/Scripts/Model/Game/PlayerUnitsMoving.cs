using UnityEngine;

namespace Model.Game
{
    internal class PlayerUnitsMoving : GameState
    {
        private float _startTime;
        private float _moveDuration = 0.5f;
        public PlayerUnitsMoving(GameModel model) : base(model)
        {
        }
        
        public override void OnEnter()
        {
            _model.DecreaseSwipesAmountLeft();
            _startTime = Time.time;
        }

        public override void UpdatePass()
        {
            SetNextState();
        }

        public override void OnExit()
        {
        }

        public override void SetNextState()
        {
            if (Time.time < _startTime + _moveDuration) 
                return;
            
            if (_model.SwipesAmountLeft > 0)
                _model.SwitchState<PlayerTurn>();
            else
                _model.SwitchState<EnemyTurn>();
        }
    }
}