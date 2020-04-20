namespace PID.Objects
{
    public class Controller
    {
        double lastInputValue;
        double inputSum;

        public Controller()
        {

        }

        public double CalculateResult(double inputValue, double dt, EmelentP p, ElementI i, ElementD d, int iteration, double maxOutputValue, double minOutputValue)
        {
            double proportionalElementResult = p.CalculateResult(inputValue);

            inputSum += inputValue;
            double integratingElementValue = i.CalculateResult(inputValue, p, dt, inputSum);

            double derivativeElementValue = 0d;

            if (iteration != 0)
                derivativeElementValue = d.CalculateResult(inputValue, p, dt, lastInputValue);

            double controllerOutputValue = proportionalElementResult + integratingElementValue + derivativeElementValue;

            lastInputValue = inputValue;

            if (controllerOutputValue > maxOutputValue)
                return maxOutputValue;
            if (controllerOutputValue < minOutputValue)
                return minOutputValue;
            return controllerOutputValue;
        }

        public void ResetSupportParameters()
        {
            lastInputValue = 0;
            inputSum = 0;
        }
    }
}
