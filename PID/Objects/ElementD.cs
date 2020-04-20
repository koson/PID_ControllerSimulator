namespace PID.Objects
{
    public class ElementD
    {
        public double Kd { get; private set; }

        public ElementD()
        {
            Kd = 1;
        }

        public ElementD(double Kd)
        {
            this.Kd = Kd;
        }
        
        public double CalculateResult(double inputValue, EmelentP P, double dt, double lastInputValue)
        {
            if (Kd == 0)
                return 0;
            else
                return 1d / dt * P.Kp * Kd * (inputValue - lastInputValue);
        }

        public void SetParametr(double kd)
        {
            this.Kd = kd;
        }

    }
}
