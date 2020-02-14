namespace AO.SpaceGame
{
    public class AttackTypeWeaponAbility : IAbility
    {
        public SpaceshipController Spaceship { get; }
        public WeaponAttackType Type { get; }

        public AttackTypeWeaponAbility(SpaceshipController spaceship, WeaponAttackType type)
        {
            Spaceship = spaceship;
            Type = type;
        }

        public void Destroy()
        {
            Spaceship.ChangeWeaponAttackTypes(WeaponAttackType.Single);
        }

        public void Start()
        {
            Spaceship.ChangeWeaponAttackTypes(Type);
        }

        public void Update()
        {
        }
    }
}
