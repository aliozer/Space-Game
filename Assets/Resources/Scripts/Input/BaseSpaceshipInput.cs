namespace AO.Input
{
    public abstract class BaseSpaceshipInput : BaseAirplaneInput
    {
        public abstract bool GetPower1KeyDown();
        public abstract bool GetPower2KeyDown();
        public abstract bool GetPower3KeyDown();
        public abstract bool GetPower4KeyDown();
        public abstract bool GetPower5KeyDown();
    }
}
