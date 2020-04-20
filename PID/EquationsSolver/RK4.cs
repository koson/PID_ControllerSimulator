using PID.DifferentialEquations;

namespace PID.EquationsSolver
{
    public class RK4
    {
        double k0, k1, k2, k3, l0, l1, l2, l3;
        public RK4()
        {
        }

        public double SolveEquations(EquationI currentEquation, EquationOmega speedEquation, double inputValue, double loadTorqueValue, ref double i, ref double omega, double dt)
        {

            k0 = dt * currentEquation.CalculateResult(i, omega, inputValue);
            l0 = dt * speedEquation.CalculateResult(i, loadTorqueValue);

            k1 = dt * currentEquation.CalculateResult(i + (dt * k0 / 2.0), omega + (dt * l0 / 2.0), inputValue);
            l1 = dt * speedEquation.CalculateResult(i + (dt * k0 / 2.0), loadTorqueValue);

            k2 = dt * currentEquation.CalculateResult(i + (dt * k1 / 2.0), omega + (dt * l1 / 2.0), inputValue);
            l2 = dt * speedEquation.CalculateResult(i + (dt * k1 / 2.0), loadTorqueValue);

            k3 = dt * currentEquation.CalculateResult(i + (dt * k2), omega + (dt * l2), inputValue);
            l3 = dt * speedEquation.CalculateResult(i + (dt * k2), loadTorqueValue);

            omega = omega + ((l0 + 2.0 * l1 + 2.0 * l2 + l3) / 6.0);
            i = i + ((k0 + 2.0 * k1 + 2.0 * k2 + k3) / 6.0);

            return omega;
        }


    }
}
