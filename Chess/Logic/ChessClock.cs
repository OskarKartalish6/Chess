using System;
using System.Windows.Threading;

namespace Chess.Logic
{
    internal class ChessClock
    {
        private readonly DispatcherTimer timer;

        public TimeSpan WhiteTime { get; private set; }
        public TimeSpan BlackTime { get; private set; }

        public int IncrementSeconds { get; }

        public PieceColor ActiveColor { get; private set; } = PieceColor.White;


        public event Action<TimeSpan, TimeSpan>? TimeChanged;
        public event Action<PieceColor>? TimeExpired;

        public ChessClock(int minutes, int incrementSeconds = 0)
        {
            WhiteTime = TimeSpan.FromMinutes(minutes);
            BlackTime = TimeSpan.FromMinutes(minutes);
            IncrementSeconds = incrementSeconds;

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += OnTick;
        }

        private void OnTick(object? sender, EventArgs e)
        {
            if (ActiveColor == PieceColor.White)
                WhiteTime -= TimeSpan.FromSeconds(1);
            else
                BlackTime -= TimeSpan.FromSeconds(1);

            TimeChanged?.Invoke(WhiteTime, BlackTime);

            if (WhiteTime <= TimeSpan.Zero)
            {
                Stop();
                TimeExpired?.Invoke(PieceColor.White);
            }
            else if (BlackTime <= TimeSpan.Zero)
            {
                Stop();
                TimeExpired?.Invoke(PieceColor.Black);
            }
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void SwitchTurn()
        {
            if (IncrementSeconds > 0)
            {
                if (ActiveColor == PieceColor.White)
                    WhiteTime += TimeSpan.FromSeconds(IncrementSeconds);
                else
                    BlackTime += TimeSpan.FromSeconds(IncrementSeconds);
            }

            ActiveColor = ActiveColor == PieceColor.White
                ? PieceColor.Black
                : PieceColor.White;

            TimeChanged?.Invoke(WhiteTime, BlackTime);
        }
    }
}
