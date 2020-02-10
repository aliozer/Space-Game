namespace AO.SpaceGame
{
    public abstract class GameState
    {
        public GameController Controller { get; }
        public GameState(GameController controller)
        {
            Controller = controller;
        }


        public abstract void Start();
        public abstract void Stop();
        public abstract void Update();
        public abstract void FixedUpdate();
    }
}
