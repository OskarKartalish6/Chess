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

namespace Chess
{
    /// <summary>
    /// Interaction logic for EndGameWnd.xaml
    /// </summary>
    public enum EndGameResult
    {
        Restart,
        NewGame
    }
    public partial class EndGameWnd : Window
    {
        public EndGameResult Result { get; private set; }

        public EndGameWnd(string reason)
        {
            InitializeComponent();
            ReasonText.Text = reason;
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            Result = EndGameResult.Restart;
            DialogResult = true;
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            Result = EndGameResult.NewGame;
            DialogResult = true;
        }
    }
}
