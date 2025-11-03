using System;
using System.Drawing;
using System.Windows.Forms;

namespace AdvancedCalculatorGUI
{
    public class HistoryForm : Form
    {
        private HistoryObserver historyObserver;
        private ListBox historyListBox;
        private Button clearButton;
        private Button saveButton;
        private Button closeButton;

        public HistoryForm(HistoryObserver observer)
        {
            historyObserver = observer;
            InitializeForm();
            LoadHistory();
        }

        private void InitializeForm()
        {
            this.Text = "Historia Obliczeń";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240);
            historyListBox = new ListBox()
            {
                Location = new Point(20, 20), Size = new Size(440, 250), Font = new Font("Arial", 10),
                BackColor = Color.White
            };
            clearButton = new Button()
            {
                Text = "Wyczyść historię", Location = new Point(20, 290), Size = new Size(120, 35),
                BackColor = Color.LightCoral, Font = new Font("Arial", 10)
            };
            saveButton = new Button()
            {
                Text = "Zapisz do pliku", Location = new Point(150, 290), Size = new Size(120, 35),
                BackColor = Color.LightGreen, Font = new Font("Arial", 10)
            };
            closeButton = new Button()
            {
                Text = "Zamknij", Location = new Point(280, 290), Size = new Size(120, 35), BackColor = Color.LightGray,
                Font = new Font("Arial", 10)
            };
            clearButton.Click += (s, e) => ClearHistory();
            saveButton.Click += (s, e) => SaveHistory();
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(historyListBox);
            this.Controls.Add(clearButton);
            this.Controls.Add(saveButton);
            this.Controls.Add(closeButton);
        }

        private void LoadHistory()
        {
            historyListBox.Items.Clear();
            var history = historyObserver.GetHistory();
            if (history.Count == 0)
            {
                historyListBox.Items.Add("Brak historii obliczeń");
                return;
            }

            for (int i = history.Count - 1; i >= 0; i--)
            {
                historyListBox.Items.Add(history[i]);
            }
        }

        private void ClearHistory()
        {
            var result = MessageBox.Show("Czy na pewno chcesz wyczyścić historię?", "Potwierdzenie",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                historyObserver.ClearHistory();
                LoadHistory();
                MessageBox.Show("Historia wyczyszczona", "Informacja", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void SaveHistory()
        {
            try
            {
                historyObserver.SaveToFile();
                MessageBox.Show("Historia zapisana do pliku 'historia.txt'", "Sukces", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisywania: {ex.Message}", "Błąd", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}