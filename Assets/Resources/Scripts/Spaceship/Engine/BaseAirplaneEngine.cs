using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO
{
    public abstract class BaseAirplaneEngine: MonoBehaviour
    {
        [SerializeField]
        protected float _maxForce = 200f;
        public float MaxForce => _maxForce;

        public float Power { get; protected set; }

        public abstract Vector3 CalculateForce(float throttle);
    }
}
