namespace Model.Game
{
    internal abstract class GameState
    {
        private protected GameModel _model;

        public GameState(GameModel model)
        {
            _model = model;
        }
        
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void SetNextState();

        public virtual void UpdatePass()
        {
        }
    }
}