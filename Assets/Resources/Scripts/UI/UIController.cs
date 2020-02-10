using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AO.SpaceGame.UI
{

    public class UIController: MonoBehaviour
    {
        public event Action Started;
        public event Action Exit;

        [SerializeField]
        private Image _description;
        public Image Description => _description;
        [SerializeField]
        private Button _startButton;
        public Button StartButton => _startButton;
        [SerializeField]
        private Button _exitButton;
        public Button ExitButton => _exitButton;
        [SerializeField]
        private ToggleButton _crossToggle;
        public ToggleButton CrossToggle => _crossToggle;
        [SerializeField]
        private ToggleButton _doubleToggle;
        public ToggleButton DoubleToggle => _doubleToggle;
        [SerializeField]
        private ToggleButton _fireRateToggle;
        public ToggleButton FireRateToggle => _fireRateToggle;
        [SerializeField]
        private ToggleButton _speedToggle;
        public ToggleButton SpeedToggle => _speedToggle;
        [SerializeField]
        private ToggleButton _ghostToggle;
        public ToggleButton GhostToggle => _ghostToggle;

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
