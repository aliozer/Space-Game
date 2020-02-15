using AO.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO.SpaceGame.Input
{
    public class KeyboardSpaceshipInput : BaseSpaceshipInput
    {

        protected override void UpdateInput()
        {
            _pitch = UnityEngine.Input.GetAxis("Vertical");
            _roll = UnityEngine.Input.GetAxis("Horizontal");
            _yaw = UnityEngine.Input.GetAxis("Yaw");
            _throttle = UnityEngine.Input.GetAxis("Throttle");
            _fire1 = UnityEngine.Input.GetKey(KeyCode.Space);
            _fire2 = UnityEngine.Input.GetKey(KeyCode.LeftControl);

        }
        public override bool GetPower1KeyDown()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.Alpha1);
        }
        public override bool GetPower2KeyDown()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.Alpha2);
        }

        public override bool GetPower3KeyDown()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.Alpha3);
        }

        public override bool GetPower4KeyDown()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.Alpha4);
        }
        public override bool GetPower5KeyDown()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.Alpha5);
        }

    }
}
