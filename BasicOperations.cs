using System;

namespace AdvancedCalculatorGUI
{
    public class Addition : IMathOperation
    {
        public string Symbol => "+";
        public string Description => "Dodawanie";
        public double Calculate(double a, double b) => a + b;
    }

    public class Subtraction : IMathOperation
    {
        public string Symbol => "-";
        public string Description => "Odejmowanie";
        public double Calculate(double a, double b) => a - b;
    }

    public class Multiplication : IMathOperation
    {
        public string Symbol => "*";
        public string Description => "Mnożenie";
        public double Calculate(double a, double b) => a * b;
    }

    public class Division : IMathOperation
    {
        public string Symbol => "/";
        public string Description => "Dzielenie";

        public double Calculate(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Nie można dzielić przez zero!");
            return a / b;
        }
    }

    public class PowerOperation : IMathOperation
    {
        public string Symbol => "^";
        public string Description => "Potęgowanie";
        public double Calculate(double a, double b) => Math.Pow(a, b);
    }

    public class SquareRootOperation : IMathOperation
    {
        public string Symbol => "sqrt";
        public string Description => "Pierwiastek kwadratowy";
        public double Calculate(double a, double b) => Math.Sqrt(a);
    }

    public class PercentageOperation : IMathOperation
    {
        public string Symbol => "%";
        public string Description => "Procent z liczby";
        public double Calculate(double a, double b) => (a * b) / 100;
    }

    public class AbsoluteValueOperation : IMathOperation
    {
        public string Symbol => "abs";
        public string Description => "Wartość bezwzględna";
        public double Calculate(double a, double b) => Math.Abs(a);
    }

    public class ModuloOperation : IMathOperation
    {
        public string Symbol => "mod";
        public string Description => "Reszta z dzielenia";

        public double Calculate(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Nie można dzielić przez zero!");
            return a % b;
        }
    }
}