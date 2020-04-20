using PID.Objects;

namespace PID.DifferentialEquations
{
    public class EquationI
    {
        public double[] EquationCoefficient { get; } //współczynniki równania

        public EquationI()
        {
        }

        public EquationI(Engine S)
        {
            EquationCoefficient = new double[3];
            EquationCoefficient[0] = (S.Rd / S.Ld); //pierwszy przy id
            EquationCoefficient[1] = (S.Ke / S.Ld); //drugi przy omega
            EquationCoefficient[2] = S.Ld; //trzeci przy Uz
        }

        public double CalculateResult(double currentValue, double speedValue, double voltageValue)
        {
            // di/dt = -... id - ... omega + ... Uz
            return -EquationCoefficient[0] * currentValue - EquationCoefficient[1] * speedValue + voltageValue / EquationCoefficient[2];
        }
    }
}
