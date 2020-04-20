using System;

namespace PID.HelperClasses.Checker
{
    class NumberChecker
    {
        public static bool TryIntParse(string stringInt, ref int inputValue)
        {
            return Int32.TryParse(stringInt, out inputValue);
        }

        public static bool TryDoubleParse(string stringDouble, ref double inputValue)
        {
            return double.TryParse(stringDouble, out inputValue);
        }
    }
}
