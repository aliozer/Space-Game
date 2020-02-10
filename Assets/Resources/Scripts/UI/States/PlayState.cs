namespace AO.SpaceGame.UI
{
    public class PlayState : UIState
    {
        public override void Handle(UIController controller)
        {
            controller.StartButton.gameObject.SetActive(false);
            controller.ExitButton.gameObject.SetActive(true);
            controller.Description.gameObject.SetActive(false);

            controller.CrossToggle.gameObject.SetActive(true);
            controller.FireRateToggle.gameObject.SetActive(true);
            controller.DoubleToggle.gameObject.SetActive(true);
            controller.SpeedToggle.gameObject.SetActive(true);
            controller.GhostToggle.gameObject.SetActive(true);
        }
    }
}
