using System;
using UnityEngine;
using UnityEngine.UI;

namespace AO.SpaceGame.UI
{
    public class AbilityButton : MonoBehaviour
    {
        public event Action<bool> Changed;

        [SerializeField]
        private Toggle _toggle;

        public bool IsOn {
            get { return _toggle.isOn; }
            set { _toggle.isOn = value; }
        }


        public bool Interactable {
            get { return _toggle.interactable; }
            set { _toggle.interactable = value; }
        }


        private void Start()
        {
            if (_toggle != null)
                _toggle.onValueChanged.AddListener(value => OnValueChanged(value));
        }

        private void OnValueChanged(bool value)
        {
            Changed?.Invoke(value);
        }
    }
}
