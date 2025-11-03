namespace AdvancedCalculatorGUI
{
    public interface IMathOperation
    {
        string Symbol { get; }
        string Description { get; }
        double Calculate(double a, double b);
    }

    public interface IStatisticalOperation : IMathOperation
    {
        bool SupportsMultipleValues { get; }
        double Calculate(params double[] values);
    }
}