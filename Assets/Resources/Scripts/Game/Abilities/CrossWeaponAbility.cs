using UnityEngine;

namespace AO.SpaceGame
{
    public class CrossWeaponAbility : IAbility
    {

        public SpaceshipController Spaceship { get; }

        public CrossWeaponAbility(SpaceshipController spaceship)
        {
            Spaceship = spaceship;
        }

        public void Start()
        {
            Spaceship.TopAssaultWeapon = AssaultWeaponFactory.GetWeapon<SpaceshipTopAssaultWeapon>(Spaceship.transform);
        }

        public void Destroy()
        {
            GameObject.DestroyImmediate(Spaceship.TopAssaultWeapon.gameObject);
        }

        public void ChangeWeaponFireSpeedFactors(float speedFactor)
        {
            Spaceship.ChangeWeaponFireSpeedFactors(speedFactor);
        }
        public void ChangeWeaponFireRates(float rate)
        {
            Spaceship.ChangeWeaponFireRates(rate);
        }
        public void ChangeWeaponAttackTypes(WeaponAttackType type)
        {
            Spaceship.ChangeWeaponAttackTypes(type);
        }
        public void Update()
        {
        }
    }
}
