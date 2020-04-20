using PID.Objects;

namespace PID.DifferentialEquations
{
    public class EquationOmega
    {
        public double[] EquationCoefficient { get; } //współczynniki równania
        public EquationOmega()
        {
        }

        public EquationOmega(Engine S)
        {
            EquationCoefficient = new double[2];
            EquationCoefficient[0] = S.Km / S.J;
            EquationCoefficient[1] = S.J;
        }

        public double CalculateResult(double currentValue, double loadTorqueValue)
        {
            // d omega/dt = ... id - ... Mobc
            return EquationCoefficient[0] * currentValue - loadTorqueValue / EquationCoefficient[1];
        }
    }
}
