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
            Spaceship.TopAssaultWeapon.AttackType = Spaceship.AttackType;
            Spaceship.TopAssaultWeapon.FireRate = Spaceship.FireRate;
            Spaceship.TopAssaultWeapon.SpeedFactor = Spaceship.SpeedFactor;
        }

        public void Destroy()
        {
            GameObject.DestroyImmediate(Spaceship.TopAssaultWeapon.gameObject);
        }

        public void Update()
        {
        }
    }
}
