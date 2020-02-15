namespace AO.SpaceGame
{
    public class AttackTypeWeaponAbility : IAbility
    {
        public SpaceshipController Spaceship { get; }

        public AttackTypeWeaponAbility(SpaceshipController spaceship)
        {
            Spaceship = spaceship;
        }

        public void Destroy()
        {
            Spaceship.ChangeWeaponAttackTypes(WeaponAttackType.Single);
        }

        public void Start()
        {
            Spaceship.ChangeWeaponAttackTypes(WeaponAttackType.Brust);
        }

        public void Update()
        {
        }
    }
}
