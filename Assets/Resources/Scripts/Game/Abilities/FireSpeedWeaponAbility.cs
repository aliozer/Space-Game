namespace AO.SpaceGame
{
    public class FireSpeedWeaponAbility : IAbility
    {
        private float _defaultSpeedFactor = 1.0f;

        public SpaceshipController Spaceship { get; }

        public FireSpeedWeaponAbility(SpaceshipController spaceship)
        {
            Spaceship = spaceship;
        }

        public void Destroy()
        {
            Spaceship.ChangeWeaponFireSpeedFactors(1.5f);
        }

        public void Start()
        {
            Spaceship.ChangeWeaponFireSpeedFactors(_defaultSpeedFactor);
        }

        public void Update()
        {
        }
    }
}
