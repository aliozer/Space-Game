namespace AO.SpaceGame.UI
{
    public class PlayState : UIState
    {
        public override void Handle(UIController controller)
        {
            controller.StartButton.gameObject.SetActive(false);
            controller.ExitButton.gameObject.SetActive(true);
            controller.Description.gameObject.SetActive(false);

            controller.AbilityController.Initialize();
        }
    }
}
