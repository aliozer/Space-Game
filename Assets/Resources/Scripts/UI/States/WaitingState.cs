namespace AO.SpaceGame.UI
{
    public class WaitingState : UIState
    {
        public override void Handle(UIController controller)
        {
            controller.StartButton.gameObject.SetActive(true);
            controller.ExitButton.gameObject.SetActive(false);
            controller.Description.gameObject.SetActive(true);

            controller.AbilityController.Clear();
        }
    }
}
