using System;
using System.Windows;
using Chess.Logic;

namespace Chess
{
    public partial class MainWindow : Window
    {
        private GameLogic game;
        private int minutes;
        private int increment;
        private int AiDepth;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        private void OnGameEnded(string reason)
        {

            Dispatcher.Invoke(() =>
            {
                var wnd = new EndGameWnd(reason)
                {
                    Owner = this
                };

                if (wnd.ShowDialog() != true)
                    return;

                if (wnd.Result == EndGameResult.Restart)
                {
                    StartGame();
                }
                else
                {
                    ShowSettingsAndStart();
                }
            });
        }
        private void ShowSettingsAndStart()
        {
            var settings = new SettingsWindow { Owner = this };

            if (settings.ShowDialog() != true)
                return;

            minutes = settings.SelectedMinutes;
            increment = settings.IncrementSeconds;
            AiDepth = settings.AiDepth;

            StartGame();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsWindow
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            if (settings.ShowDialog() != true)
            {
                Close();
                return;
            }

            minutes = settings.SelectedMinutes;
            increment = settings.IncrementSeconds;
            AiDepth = settings.AiDepth;
            StartGame();
        }
        private void StartGame()
        {
            BoardGrid.Children.Clear();

            game = new GameLogic(BoardGrid, minutes, increment, AiDepth);
            game.GameEnded += OnGameEnded;
        }


    }

}