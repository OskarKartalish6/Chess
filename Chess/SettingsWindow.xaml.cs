using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Chess.Logic;

namespace Chess
{
    public partial class SettingsWindow : Window
    {
        public int SelectedMinutes { get; private set; }
        public int IncrementSeconds { get; private set; }

        public bool PlayWithAI {  get; private set; }
        public int AiDepth { get; private set; } = 0;

        public SettingsWindow()
        {
            InitializeComponent();
            AiLevelComboBox.Visibility = Visibility.Collapsed;
        }

        private void TimeButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var parts = btn?.Tag?.ToString()?.Split('|');

            SelectedMinutes = int.Parse(parts[0]);
            IncrementSeconds = int.Parse(parts[1]);

            DialogResult = true;
            Close();
        }

        private void AiCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PlayWithAI = true;
            AiLevelComboBox.Visibility = Visibility.Visible;
        }

        private void AiCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PlayWithAI = false;
            AiLevelComboBox.SelectedItem = null;
            AiDepth = 0;
            AiLevelComboBox.Visibility = Visibility.Collapsed;
        }

        private void AiLevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!PlayWithAI) return;

            if (AiLevelComboBox.SelectedItem == null) return;

            var item = (ComboBoxItem)AiLevelComboBox.SelectedItem;
            AiDepth = int.Parse(item.Tag.ToString());
        }

    }
}
