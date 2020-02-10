using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO.Input
{
    public abstract class BaseAirplaneInput : MonoBehaviour
    {
        protected float _pitch = 0f;
        public float Picth => _pitch;

        protected float _roll = 0f;
        public float Roll => _roll;

        protected float _yaw = 0f;
        public float Yaw => _yaw;

        protected float _throttle = 0f;
        public float Throttle => _throttle;

        protected bool _fire1;
        public bool Fire1 => _fire1;

        protected bool _fire2;
        public bool Fire2 => _fire2;

        protected void Update()
        {
            UpdateInput();
        }

        protected abstract void UpdateInput();
    }
}
