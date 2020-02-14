namespace AO.SpaceGame
{
    public class IncreaseFireRateWeaponAbility : IAbility
    {
        private float _defaultRate;

        public SpaceshipController Spaceship { get; }
        public float Rate { get; }

        public IncreaseFireRateWeaponAbility(SpaceshipController spaceship, float rate)
        {
            _defaultRate = spaceship.FireRate;
            Spaceship = spaceship;
            Rate = rate;
        }

        public void Destroy()
        {
            Spaceship.ChangeWeaponFireRates(Rate);
        }

        public void Start()
        {
            Spaceship.ChangeWeaponFireRates(_defaultRate);
        }

        public void Update()
        {
        }
    }
}
