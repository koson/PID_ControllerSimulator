namespace PID.Objects
{
    public class Engine
    {
        public string Name { get; private set; }
        public double Rd { get; private set; }
        public double Ld { get; private set; }
        public double J { get; private set; }
        public double Ke { get; private set; }
        public double Km { get; private set; }

        public Engine()
        {
            Name = "Przykladowy silnik z parametrami na 1";
            Rd = 1;
            Ld = 1;
            J = 1;
            Ke = 1;
            Km = 1;
        }

        public Engine(string name, double engineResistanceValue, double engineInductanceValue, double engineInertiaValue, double Ke, double Km)
        {
            this.Name = name;
            Rd = engineResistanceValue;
            Ld = engineInductanceValue;
            J = engineInertiaValue;
            this.Ke = Ke;
            this.Km = Km;
        }

        public void SetParameters(double resistance, double inductance, double inertia, double Ke, double Km)
        {
            Rd = resistance;
            Ld = inductance;
            J = inertia;
            this.Ke = Ke;
            this.Km = Km;
        }
    }
}
