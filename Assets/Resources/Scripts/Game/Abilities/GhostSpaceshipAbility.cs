using AO.Utilities;
using UnityEngine;

namespace AO.SpaceGame
{
    public class GhostSpaceshipAbility : IAbility
    {
        private SpaceshipController _ghostSpaceShip;
        private float _speedFactor;
        private float _rate;
        private WeaponAttackType _type;

        public SpaceshipController Spaceship { get; }
        public bool IsCrossWeaponEnabled { get; }

        public GhostSpaceshipAbility(SpaceshipController spaceship, bool isCrossWeaponEnabled)
        {
            Spaceship = spaceship;
            IsCrossWeaponEnabled = isCrossWeaponEnabled;
        }
        public void Destroy()
        {
            GameObject.Destroy(_ghostSpaceShip.gameObject);
        }

        public void Start()
        {
            var spaceshipPrefab = PrefabUtil.Create<SpaceshipController>("GhostSpaceship");
            _ghostSpaceShip = GameObject.Instantiate(spaceshipPrefab, Spaceship.transform.position + Spaceship.transform.up, Spaceship.transform.rotation);
            _ghostSpaceShip.Input = Spaceship.Input;

            _ghostSpaceShip.ChangeWeaponFireSpeedFactors(_speedFactor);
            _ghostSpaceShip.ChangeWeaponFireRates(_rate);
            _ghostSpaceShip.ChangeWeaponAttackTypes(_type);

            if (IsCrossWeaponEnabled)
            {
                _ghostSpaceShip.TopAssaultWeapon = AssaultWeaponFactory.GetWeapon<SpaceshipTopAssaultWeapon>(_ghostSpaceShip.transform, "GhostSpaceshipTopAssaultWeapon");
            }
        }

        public void ChangeWeaponFireSpeedFactors(float speedFactor)
        {
            _speedFactor = speedFactor;
        }
        public void ChangeWeaponFireRates(float rate)
        {
            _rate = rate;
            
        }
        public void ChangeWeaponAttackTypes(WeaponAttackType type)
        {
            _type = type;
        }

        public void Update()
        {
            Vector3 _currentVelocity = Vector3.zero;
            _ghostSpaceShip.transform.position = Vector3.SmoothDamp(_ghostSpaceShip.transform.position, Spaceship.transform.position + Spaceship.transform.up * 4f, ref _currentVelocity, Time.deltaTime * 5f);
            _ghostSpaceShip.transform.rotation = Quaternion.Slerp(_ghostSpaceShip.transform.rotation, Spaceship.transform.rotation, Time.deltaTime * 20f);
        }
    }
}
