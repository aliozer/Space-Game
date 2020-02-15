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

        private bool _isStartedEngine;

        public SpaceshipMissileWeapon MissileWeapon { get => _missileWeapon; set => _missileWeapon = value; }

        public float FireRate { get; set; } = 2.0f;
        public float SpeedFactor { get; set; } = 1.0f;
        public WeaponAttackType AttackType { get; set; } = WeaponAttackType.Single;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_isStartedEngine)
            {
                SetThrustersForce();
            }

        }

        protected override void Update()
        {
            base.Update();

            WeaponsFire();
        }

        public void StartEngine()
        {
            _isStartedEngine = true;
        }


        public void ChangeWeaponFireRates(float rate)
        {
            FireRate = rate;

            if (_frontAssaultWeapon)
                _frontAssaultWeapon.FireRate = FireRate;

            if (_backAssaultWeapon)
                _backAssaultWeapon.FireRate = FireRate;

            if (_topAssaultWeapon)
                _topAssaultWeapon.FireRate = FireRate;

            if (_missileWeapon)
                _missileWeapon.FireRate = FireRate;
        }

        public void ChangeWeaponAttackTypes(WeaponAttackType weaponAttackType)
        {
            AttackType = weaponAttackType;

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
            return _isStartedEngine && Input ? Input.Throttle : 0;
        }


        protected override float GetPitch()
        {
            return _isStartedEngine && Input ? Input.Picth : 0;
        }

        protected override float GetRoll()
        {
            return _isStartedEngine && Input ? -Input.Roll : 0;
        }

        protected override float GetYaw()
        {
            return _isStartedEngine && Input ? Input.Yaw : 0;
        }
    }
}
