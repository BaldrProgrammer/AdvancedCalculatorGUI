using System;
using System.Collections.Generic;

namespace AdvancedCalculatorGUI
{
    public class Calculator
    {
        private readonly Dictionary<string, IMathOperation> _operations;
        private readonly List<ICalculationObserver> _observers;

        public Calculator()
        {
            _operations = new Dictionary<string, IMathOperation>();
            _observers = new List<ICalculationObserver>();
            RegisterOperation(new Addition());
            RegisterOperation(new Subtraction());
            RegisterOperation(new Multiplication());
            RegisterOperation(new Division());
            RegisterOperation(new PowerOperation());
        }

        public void RegisterOperation(IMathOperation operation)
        {
            _operations[operation.Symbol] = operation;
        }

        public void AddObserver(ICalculationObserver observer)
        {
            _observers.Add(observer);
        }

        public double PerformOperation(double a, double b, string opSymbol)
        {
            if (!_operations.ContainsKey(opSymbol)) throw new ArgumentException($"Nieznana operacja: {opSymbol}");
            var result = _operations[opSymbol].Calculate(a, b);
            string calculation = $"{a} {opSymbol} {b} = {result}";
            foreach (var observer in _observers)
            {
                observer.OnCalculationPerformed(calculation);
            }

            return result;
        }
    }
}