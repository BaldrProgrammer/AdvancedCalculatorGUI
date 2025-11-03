using System;
using System.Collections.Generic;
using System.IO;

namespace AdvancedCalculatorGUI
{
    public interface ICalculationObserver
    {
        void OnCalculationPerformed(string calculation);
    }

    public class HistoryObserver : ICalculationObserver
    {
        private readonly List<string> _history = new List<string>();

        public void OnCalculationPerformed(string calculation)
        {
            _history.Add($"{DateTime.Now:HH:mm:ss} - {calculation}");
            if (_history.Count > 50) _history.RemoveAt(0);
        }

        public List<string> GetHistory()
        {
            return new List<string>(_history);
        }

        public void SaveToFile(string filename = "historia.txt")
        {
            File.WriteAllLines(filename, _history);
        }

        public void ClearHistory()
        {
            _history.Clear();
        }
    }
}