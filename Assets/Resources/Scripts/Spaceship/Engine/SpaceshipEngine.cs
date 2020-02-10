using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO.SpaceGame
{
    public class SpaceshipEngine : BaseAirplaneEngine
    {
        [SerializeField]
        private AnimationCurve _powerCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public override Vector3 CalculateForce(float throttle)
        {
            throttle = Mathf.Clamp01(throttle);
            throttle = _powerCurve.Evaluate(throttle);

            Power = throttle * MaxForce;

            Vector3 force = transform.forward * Power;

            return force;
        }
    }
}
