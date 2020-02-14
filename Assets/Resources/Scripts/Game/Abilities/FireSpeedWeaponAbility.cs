namespace AO.SpaceGame
{
    public class FireSpeedWeaponAbility : IAbility
    {
        private float _speedFactor;

        public SpaceshipController Spaceship { get; }
        public float SpeedFactor { get; }

        public FireSpeedWeaponAbility(SpaceshipController spaceship, float speedFactor)
        {
            _speedFactor = spaceship.SpeedFactor;
            Spaceship = spaceship;
            SpeedFactor = speedFactor;
        }

        public void Destroy()
        {
            Spaceship.ChangeWeaponFireSpeedFactors(SpeedFactor);
        }

        public void Start()
        {
            Spaceship.ChangeWeaponFireSpeedFactors(_speedFactor);
        }

        public void Update()
        {
        }
    }
}
