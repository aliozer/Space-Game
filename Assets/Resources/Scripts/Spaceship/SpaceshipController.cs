using AO.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO.SpaceGame
{
    public class SpaceshipController : BaseAirplaneController
    {

        [Header("Input")]
        [SerializeField]
        private BaseAirplaneInput _input;
        public BaseAirplaneInput Input { get => _input; set => _input = value; }

        [Header("Thruster")]
        [SerializeField]
        public List<SpaceshipThruster> _thrusters = new List<SpaceshipThruster>();

        [Header("FPS Camera")]
        [SerializeField]
        public Camera _fpsCamera;

        private float _cameraDistance = -20f;
        private float _cameraHeight = 6f;


        [Header("Weapon Slots")]
        [SerializeField]
        private SpaceshipFrontAssaultWeapon _frontAssaultWeapon;
        public SpaceshipFrontAssaultWeapon FrontAssaltWeapon { get => _frontAssaultWeapon; set => _frontAssaultWeapon = value; }
        [SerializeField]
        private SpaceshipBackAssaultWeapon _backAssaultWeapon;
        public SpaceshipBackAssaultWeapon BackAssaultWeapon { get => _backAssaultWeapon; set => _backAssaultWeapon = value; }
        [SerializeField]
        private SpaceshipTopAssaultWeapon _topAssaultWeapon;
        public SpaceshipTopAssaultWeapon TopAssaultWeapon { get => _topAssaultWeapon; set => _topAssaultWeapon = value; }
        [SerializeField]
        private SpaceshipMissileWeapon _missileWeapon;
        private Vector3 _velocity;
        private Vector3 _angularVelocity;

        public SpaceshipMissileWeapon MissileWeapon { get => _missileWeapon; set => _missileWeapon = value; }

        public Vector3 Velocity => _rigidbody.velocity;
        public Vector3 AngularVelocity => _rigidbody.angularVelocity;

        protected override void Start()
        {
            base.Start();

            if (_fpsCamera)
                _fpsCamera.transform.localPosition = new Vector3(0f, _cameraHeight, _cameraDistance);

            _rigidbody.velocity = _velocity;
            _rigidbody.angularVelocity = _angularVelocity;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            SetThrustersForce();
            UpdateCameraPositionAndRotation();

        }

        protected override void Update()
        {
            base.Update();

            WeaponsFire();
        }

        public void SetVelocity(Vector3 velocity, Vector3 angularVelocity)
        {
            _velocity = velocity;
            _angularVelocity = angularVelocity;
            if (_rigidbody)
            {
                _rigidbody.velocity = _velocity;
                _rigidbody.angularVelocity = _angularVelocity;
            }
        }

        public void ChangeWeaponFireRates(float rate)
        {
            if (_frontAssaultWeapon)
                _frontAssaultWeapon.FireRate = rate;

            if (_backAssaultWeapon)
                _backAssaultWeapon.FireRate = rate;

            if (_topAssaultWeapon)
                _topAssaultWeapon.FireRate = rate;

            if (_missileWeapon)
                _missileWeapon.FireRate = rate;
        }

        public void ChangeWeaponAttackTypes(WeaponAttackType weaponAttackType)
        {
            if (_frontAssaultWeapon)
                _frontAssaultWeapon.AttackType = weaponAttackType;

            if (_backAssaultWeapon)
                _backAssaultWeapon.AttackType = weaponAttackType;

            if (_topAssaultWeapon)
                _topAssaultWeapon.AttackType = weaponAttackType;

            if (_missileWeapon)
                _missileWeapon.AttackType = weaponAttackType;
        }

        private void WeaponsFire()
        {
            if (Input && Input.Fire1 && GetWeaponsReady())
            {
                if (_frontAssaultWeapon)
                    _frontAssaultWeapon.Shoot();

                if (_backAssaultWeapon)
                    _backAssaultWeapon.Shoot();

                if (_topAssaultWeapon)
                    _topAssaultWeapon.Shoot();
            }

            if (Input && Input.Fire2)
            {
                if (_missileWeapon)
                    _missileWeapon.Shoot();
            }
        }

        private bool GetWeaponsReady()
        {
            bool result = true;

            if (_frontAssaultWeapon)
                result &= _frontAssaultWeapon.IsReady;

            if (_backAssaultWeapon)
                result &= _backAssaultWeapon.IsReady;

            if (_topAssaultWeapon)
                result &= _topAssaultWeapon.IsReady;

            return result;
        }

        internal void ChangeWeaponFireSpeedFactors(float speedFactor)
        {
            if (_frontAssaultWeapon)
                _frontAssaultWeapon.SpeedFactor = speedFactor;

            if (_backAssaultWeapon)
                _backAssaultWeapon.SpeedFactor = speedFactor;

            if (_topAssaultWeapon)
                _topAssaultWeapon.SpeedFactor = speedFactor;

            if (_missileWeapon)
                _missileWeapon.SpeedFactor = speedFactor;
        }

        private void UpdateCameraPositionAndRotation()
        {
            if (_fpsCamera == null)
                return;

            float smoothFactor = 2.0f;
            float angle = 18f;

            var cameraEuler = Quaternion.Euler(0f, 0f, -GetRoll() * angle);
            _fpsCamera.transform.localRotation = Quaternion.Lerp(_fpsCamera.transform.localRotation, cameraEuler, Time.deltaTime * smoothFactor);

            var cameraParentEuler = Quaternion.Euler(-GetPitch() * angle, -GetYaw() * angle, 0f);
            _fpsCamera.transform.parent.localRotation = Quaternion.Lerp(_fpsCamera.transform.parent.localRotation, cameraParentEuler, Time.deltaTime * smoothFactor);

            _fpsCamera.transform.localPosition = Vector3.Lerp(_fpsCamera.transform.localPosition, new Vector3(0f, _cameraHeight, _cameraDistance - (angle * GetThrottle())), Time.deltaTime * smoothFactor);
        }

        private void SetThrustersForce()
        {
            if (Input)
            {
                float power = 0.0f;

                foreach (var engine in Engines)
                {
                    power += (engine.Power / engine.MaxForce);
                }

                power /= Engines.Count;

                foreach (var thruster in _thrusters)
                {
                    thruster.AddForce(power);
                }
            }
        }

        protected override float GetThrottle()
        {
            return Input ? Input.Throttle : 0;
        }


        protected override float GetPitch()
        {
            return Input ? -Input.Picth : 0;
        }

        protected override float GetRoll()
        {
            return Input ? -Input.Roll : 0;
        }

        protected override float GetYaw()
        {
            return Input ? Input.Yaw : 0;
        }
    }
}
