namespace AO.SpaceGame.UI
{
    public class WaitingState : UIState
    {
        public override void Handle(UIController controller)
        {
            controller.StartButton.gameObject.SetActive(true);
            controller.ExitButton.gameObject.SetActive(false);
            controller.Description.gameObject.SetActive(true);

            controller.CrossToggle.IsOn = false;
            controller.CrossToggle.Interactable = true;
            controller.FireRateToggle.IsOn = false;
            controller.FireRateToggle.Interactable = true;
            controller.DoubleToggle.IsOn = false;
            controller.DoubleToggle.Interactable = true;
            controller.SpeedToggle.IsOn = false;
            controller.SpeedToggle.Interactable = true;
            controller.GhostToggle.IsOn = false;
            controller.GhostToggle.Interactable = true;

            controller.CrossToggle.gameObject.SetActive(false);
            controller.FireRateToggle.gameObject.SetActive(false);
            controller.DoubleToggle.gameObject.SetActive(false);
            controller.SpeedToggle.gameObject.SetActive(false);
            controller.GhostToggle.gameObject.SetActive(false);
        }
    }
}
