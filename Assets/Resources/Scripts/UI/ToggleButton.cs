using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AO.SpaceGame.UI
{
    public class ToggleButton: MonoBehaviour
    {
        private Toggle _toggle;

        public bool IsOn {
            get { return _toggle.isOn; }
            set { _toggle.isOn = value; }
        }

        public bool Interactable {
            get { return _toggle.interactable; }
            set { _toggle.interactable = value; }
        }

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        public void Toggle()
        {
            if (_toggle.interactable)
                _toggle.isOn = !_toggle.isOn;
        }
    }
}
