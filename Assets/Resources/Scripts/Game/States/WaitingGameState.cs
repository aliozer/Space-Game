using UnityEngine;

namespace AO.SpaceGame
{
    public class WaitingGameState : GameState
    {
        public WaitingGameState(GameController controller) : base(controller) { }

        public override void Start()
        {
            Controller.Input.enabled = false;
            Controller.AstroidController.Clear();
            Controller.UIController.Started += UIController_Started;
        }


        private void UIController_Started()
        {
            Controller.State = new PlayState(Controller);
        }

        public override void Update() { }

        public override void Stop()
        {
            Controller.UIController.Started -= UIController_Started;
        }
    }
}
