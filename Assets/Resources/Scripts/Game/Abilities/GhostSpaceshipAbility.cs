
using AO.Utilities;
using UnityEngine;

namespace AO.SpaceGame
{
    public class GhostSpaceshipAbility : IAbility
    {
        private SpaceshipController _ghostSpaceShip;
        public SpaceshipController Spaceship { get; }

        public GhostSpaceshipAbility(SpaceshipController spaceship)
        {
            Spaceship = spaceship;
        }

        public void Destroy()
        {
            if (_ghostSpaceShip != null)
                GameObject.Destroy(_ghostSpaceShip.gameObject);
        }

        public void Start()
        {
            var spaceshipPrefab = PrefabUtil.Create<SpaceshipController>("GhostSpaceship");
            _ghostSpaceShip = GameObject.Instantiate(spaceshipPrefab, Spaceship.transform.position + Spaceship.transform.up, Spaceship.transform.rotation);
            _ghostSpaceShip.Input = Spaceship.Input;


            UpdateTopAssaultWeapon();
        }

        private void UpdateTopAssaultWeapon()
        {
            if (Spaceship.TopAssaultWeapon != null && _ghostSpaceShip.TopAssaultWeapon == null)
                _ghostSpaceShip.TopAssaultWeapon = AssaultWeaponFactory.GetWeapon<SpaceshipTopAssaultWeapon>(_ghostSpaceShip.transform, "GhostSpaceshipTopAssaultWeapon");
            else if (Spaceship.TopAssaultWeapon == null && _ghostSpaceShip.TopAssaultWeapon != null)
                GameObject.Destroy(_ghostSpaceShip.TopAssaultWeapon.gameObject);

            if (_ghostSpaceShip != null)
            {
                _ghostSpaceShip.ChangeWeaponFireRates(Spaceship.FireRate);
                _ghostSpaceShip.ChangeWeaponAttackTypes(Spaceship.AttackType);
                _ghostSpaceShip.ChangeWeaponFireSpeedFactors(Spaceship.SpeedFactor);
            }
        }

        public void Update()
        {
            UpdateTopAssaultWeapon();

            _ghostSpaceShip.transform.position = Spaceship.transform.position + Spaceship.transform.up * 4f;
            _ghostSpaceShip.transform.rotation = Spaceship.transform.rotation;
        }
    }
}
