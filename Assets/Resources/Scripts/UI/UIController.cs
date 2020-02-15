using System;
using UnityEngine;
using UnityEngine.UI;

namespace AO.SpaceGame.UI
{

    public class UIController : MonoBehaviour
    {
        public event Action Started;
        public event Action Exit;

        [SerializeField]
        private AbilityController _abilityController;
        public AbilityController AbilityController => _abilityController;

        [SerializeField]
        private Image _description;
        public Image Description => _description;
        [SerializeField]
        private Button _startButton;
        public Button StartButton => _startButton;
        [SerializeField]
        private Button _exitButton;
        public Button ExitButton => _exitButton;

        private UIState state;

        public UIState State {
            get { return state; }
            set {
                state = value;

                state.Handle(this);
            }
        }

        private void Start()
        {


            _startButton.onClick.AddListener(delegate { OnStartButtonClick(); });
            _exitButton.onClick.AddListener(delegate { OnExitButtonClick(); });

            State = new WaitingState();
        }

        private void OnExitButtonClick()
        {
            Exit?.Invoke();
            State = new WaitingState();
        }

        private void OnStartButtonClick()
        {
            Started?.Invoke();
            State = new PlayState();
        }

    }
}
