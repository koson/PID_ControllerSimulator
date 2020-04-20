namespace PID.Objects
{
    public class ElementI
    {
        public double Ki { get; private set; }

        public ElementI()
        {
            Ki = 1;
        }

        public ElementI(double Ki)
        {
            this.Ki = Ki;
        }

        public double CalculateResult(double inputValue, EmelentP p, double dt, double inputSum)
        {
            if (Ki == 0)
                return 0;
            return p.Kp * dt / Ki * inputSum;
        }

        public void SetParametr(double ki)
        {
            this.Ki = ki;
        }

    }
}
