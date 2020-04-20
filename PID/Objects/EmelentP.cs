namespace PID.Objects
{
    public class EmelentP
    {
        public double Kp { get; private set; }
        public EmelentP()
        {
            Kp = 1;
        }

        public EmelentP(double Kp)
        {
            this.Kp = Kp;
        }

        public double CalculateResult(double inputValue)
        {
            return Kp * inputValue;
        }

        public void SetParametr(double kp)
        {
            this.Kp = kp;
        }
    }
}
