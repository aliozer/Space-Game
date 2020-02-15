namespace AO.SpaceGame
{
    public class IncreaseFireRateWeaponAbility : IAbility
    {
        private float _defaultRate = 2.0f;

        public SpaceshipController Spaceship { get; }

        public IncreaseFireRateWeaponAbility(SpaceshipController spaceship)
        {
            Spaceship = spaceship;
        }

        public void Destroy()
        {
            Spaceship.ChangeWeaponFireRates(1.0f);
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
