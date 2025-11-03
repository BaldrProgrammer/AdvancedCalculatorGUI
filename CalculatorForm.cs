using System;
using System.Drawing;
using System.Windows.Forms;

namespace AdvancedCalculatorGUI
{
    public partial class CalculatorForm : Form
    {
        private Calculator calculator;
        private HistoryObserver historyObserver;
        private double currentValue = 0;
        private string currentOperation = "";
        private bool newOperation = true;
        private TextBox displayTextBox;
        private ListBox historyListBox;
        private Panel buttonPanel;

        public CalculatorForm()
        {
            InitializeCalculator();
            SetupUI();
        }

        private void InitializeCalculator()
        {
            calculator = new Calculator();
            historyObserver = new HistoryObserver();
            calculator.RegisterOperation(new Addition());
            calculator.RegisterOperation(new Subtraction());
            calculator.RegisterOperation(new Multiplication());
            calculator.RegisterOperation(new Division());
            calculator.RegisterOperation(new PowerOperation());
            calculator.RegisterOperation(new SquareRootOperation());
            calculator.RegisterOperation(new PercentageOperation());
            calculator.RegisterOperation(new AbsoluteValueOperation());
            calculator.RegisterOperation(new ModuloOperation());
            calculator.AddObserver(historyObserver);
        }

        private void SetupUI()
        {
            this.Text = "Zaawansowany Kalkulator";
            this.Size = new Size(450, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            CreateDisplay();
            CreateButtonPanel();
            CreateHistoryList();
            this.Controls.Add(displayTextBox);
            this.Controls.Add(buttonPanel);
            this.Controls.Add(historyListBox);
        }

        private void CreateDisplay()
        {
            displayTextBox = new TextBox()
            {
                Location = new Point(20, 20), Size = new Size(390, 50), Font = new Font("Arial", 18),
                TextAlign = HorizontalAlignment.Right, ReadOnly = true, BackColor = Color.White, Text = "0",
                ForeColor = Color.Black
            };
        }

        private void CreateButtonPanel()
        {
            buttonPanel = new Panel()
                { Location = new Point(20, 90), Size = new Size(390, 400), BackColor = Color.Transparent };
            CreateButtons();
        }

        private void CreateHistoryList()
        {
            historyListBox = new ListBox()
            {
                Location = new Point(20, 500), Size = new Size(390, 60), Font = new Font("Arial", 9),
                BackColor = Color.White
            };
        }

        private void CreateButtons()
        {
            var buttonDefinitions = new[]
            {
                new { Text = "C", Row = 0, Col = 0, Color = Color.LightCoral, ColSpan = 1 },
                new { Text = "±", Row = 0, Col = 1, Color = Color.LightGray, ColSpan = 1 },
                new { Text = "%", Row = 0, Col = 2, Color = Color.LightGray, ColSpan = 1 },
                new { Text = "÷", Row = 0, Col = 3, Color = Color.Orange, ColSpan = 1 },
                new { Text = "√", Row = 0, Col = 4, Color = Color.LightGreen, ColSpan = 1 },
                new { Text = "7", Row = 1, Col = 0, Color = Color.White, ColSpan = 1 },
                new { Text = "8", Row = 1, Col = 1, Color = Color.White, ColSpan = 1 },
                new { Text = "9", Row = 1, Col = 2, Color = Color.White, ColSpan = 1 },
                new { Text = "×", Row = 1, Col = 3, Color = Color.Orange, ColSpan = 1 },
                new { Text = "x²", Row = 1, Col = 4, Color = Color.LightGreen, ColSpan = 1 },
                new { Text = "4", Row = 2, Col = 0, Color = Color.White, ColSpan = 1 },
                new { Text = "5", Row = 2, Col = 1, Color = Color.White, ColSpan = 1 },
                new { Text = "6", Row = 2, Col = 2, Color = Color.White, ColSpan = 1 },
                new { Text = "-", Row = 2, Col = 3, Color = Color.Orange, ColSpan = 1 },
                new { Text = "1/x", Row = 2, Col = 4, Color = Color.LightGreen, ColSpan = 1 },
                new { Text = "1", Row = 3, Col = 0, Color = Color.White, ColSpan = 1 },
                new { Text = "2", Row = 3, Col = 1, Color = Color.White, ColSpan = 1 },
                new { Text = "3", Row = 3, Col = 2, Color = Color.White, ColSpan = 1 },
                new { Text = "+", Row = 3, Col = 3, Color = Color.Orange, ColSpan = 1 },
                new { Text = "mod", Row = 3, Col = 4, Color = Color.LightGreen, ColSpan = 1 },
                new { Text = "0", Row = 4, Col = 0, Color = Color.White, ColSpan = 2 },
                new { Text = ".", Row = 4, Col = 2, Color = Color.White, ColSpan = 1 },
                new { Text = "=", Row = 4, Col = 3, Color = Color.Orange, ColSpan = 1 },
                new { Text = "Historia", Row = 4, Col = 4, Color = Color.LightBlue, ColSpan = 1 }
            };
            int buttonWidth = 70;
            int buttonHeight = 60;
            int margin = 5;
            foreach (var def in buttonDefinitions)
            {
                int colSpan = def.ColSpan ?? 1;
                var button = new Button()
                {
                    Text = def.Text, Size = new Size(colSpan * buttonWidth + (colSpan - 1) * margin, buttonHeight),
                    Location = new Point(def.Col * (buttonWidth + margin), def.Row * (buttonHeight + margin)),
                    BackColor = def.Color, Font = new Font("Arial", 12), Tag = def.Text, ForeColor = Color.Black
                };
                button.Click += Button_Click;
                buttonPanel.Controls.Add(button);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            string buttonText = button.Tag.ToString();
            try
            {
                if (char.IsDigit(buttonText[0]))
                {
                    HandleDigitInput(buttonText);
                }
                else if (buttonText == ".")
                {
                    HandleDecimalPoint();
                }
                else if (buttonText == "C")
                {
                    ClearCalculator();
                }
                else if (buttonText == "±")
                {
                    ChangeSign();
                }
                else if (IsBasicOperation(buttonText))
                {
                    HandleOperation(buttonText);
                }
                else if (buttonText == "=")
                {
                    PerformCalculation();
                }
                else if (buttonText == "√")
                {
                    PerformSquareRoot();
                }
                else if (buttonText == "x²")
                {
                    PerformSquare();
                }
                else if (buttonText == "1/x")
                {
                    PerformReciprocal();
                }
                else if (buttonText == "mod")
                {
                    HandleOperation("mod");
                }
                else if (buttonText == "Historia")
                {
                    ShowHistory();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearCalculator();
            }
        }

        private void HandleDigitInput(string digit)
        {
            if (newOperation)
            {
                displayTextBox.Text = digit;
                newOperation = false;
            }
            else
            {
                if (displayTextBox.Text == "0") displayTextBox.Text = digit;
                else displayTextBox.Text += digit;
            }
        }

        private void HandleDecimalPoint()
        {
            if (newOperation)
            {
                displayTextBox.Text = "0.";
                newOperation = false;
            }
            else if (!displayTextBox.Text.Contains("."))
            {
                displayTextBox.Text += ".";
            }
        }

        private void HandleOperation(string operation)
        {
            if (!newOperation && currentOperation != "")
            {
                PerformCalculation();
            }

            currentValue = double.Parse(displayTextBox.Text);
            currentOperation = ConvertOperationSymbol(operation);
            newOperation = true;
        }

        private void PerformCalculation()
        {
            if (currentOperation == "" || newOperation) return;
            double secondValue = double.Parse(displayTextBox.Text);
            double result = calculator.PerformOperation(currentValue, secondValue, currentOperation);
            displayTextBox.Text = FormatResult(result);
            UpdateHistoryList();
            currentOperation = "";
            newOperation = true;
        }

        private void PerformSquareRoot()
        {
            double value = double.Parse(displayTextBox.Text);
            if (value < 0)
            {
                MessageBox.Show("Nie można obliczyć pierwiastka z liczby ujemnej!", "Błąd", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            double result = calculator.PerformOperation(value, 0, "sqrt");
            displayTextBox.Text = FormatResult(result);
            UpdateHistoryList();
            newOperation = true;
        }

        private void PerformSquare()
        {
            double value = double.Parse(displayTextBox.Text);
            double result = calculator.PerformOperation(value, 2, "^");
            displayTextBox.Text = FormatResult(result);
            UpdateHistoryList();
            newOperation = true;
        }

        private void PerformReciprocal()
        {
            double value = double.Parse(displayTextBox.Text);
            if (value == 0)
            {
                MessageBox.Show("Nie można obliczyć odwrotności zera!", "Błąd", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            displayTextBox.Text = FormatResult(1.0 / value);
            newOperation = true;
        }

        private void ClearCalculator()
        {
            displayTextBox.Text = "0";
            currentValue = 0;
            currentOperation = "";
            newOperation = true;
        }

        private void ChangeSign()
        {
            if (displayTextBox.Text != "0" && displayTextBox.Text != "0.")
            {
                double value = double.Parse(displayTextBox.Text);
                displayTextBox.Text = (-value).ToString();
            }
        }

        private void ShowHistory()
        {
            var historyForm = new HistoryForm(historyObserver);
            historyForm.ShowDialog();
        }

        private void UpdateHistoryList()
        {
            historyListBox.Items.Clear();
            var history = historyObserver.GetHistory();
            for (int i = history.Count - 1; i >= 0 && i >= history.Count - 3; i--)
            {
                historyListBox.Items.Add(history[i]);
            }
        }

        private bool IsBasicOperation(string text)
        {
            return text == "+" || text == "-" || text == "×" || text == "÷" || text == "%";
        }

        private string ConvertOperationSymbol(string uiSymbol)
        {
            switch (uiSymbol)
            {
                case "×": return "*";
                case "÷": return "/";
                case "+": return "+";
                case "-": return "-";
                case "%": return "%";
                default: return uiSymbol;
            }
        }

        private string FormatResult(double result)
        {
            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                return "Błąd";
            }

            return result.ToString("G15");
        }
    }
}