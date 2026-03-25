using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Chess.Pieces;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Chess.PromotionWindow;


namespace Chess.Logic
{
    public class GameLogic
    {
        public Grid BoardGrid;
        private readonly Border[,] cells = new Border[8, 8];
        private readonly Piece?[,] board = new Piece[8, 8];

        private PieceColor walkingСolor = PieceColor.White;

        private (int row, int col)? _selectedCell = null;

        private (int row, int col) _whiteKingPos = (7, 4);
        private (int row, int col) _blackKingPos = (0, 4);

        public event Action<string>? GameEnded;

        private ChessClock clock;

        private bool _gameOver = false;
        private bool _clockStarted = false;

        private Label whiteTimerLabel;
        private Label blackTimerLabel;

        private AI? _ai;

        public GameLogic(Grid boardGrid, int minutes, int increment, int AiDepth)
        {
            BoardGrid = boardGrid;

            clock = new ChessClock(minutes, increment);
            clock.TimeChanged += OnTimeChanged;
            clock.TimeExpired += OnTimeExpired;

            if (AiDepth > 0)
                _ai = new AI(this, PieceColor.Black, AiDepth);

            InitializeBoard();
            SetupPieces();
            RenderPieces();
        }

        public void InitializeBoard()
        {
            BoardGrid.Children.Clear();

            // Создание клеток
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var cell = new Border
                    {
                        Background = (row + col) % 2 == 0 ? Brushes.Beige : Brushes.SaddleBrown,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        Tag = (row, col)
                    };
                    cell.MouseDown += Cell_Click;

                    Grid.SetRow(cell, row + 1);
                    Grid.SetColumn(cell, col);
                    BoardGrid.Children.Add(cell);
                    cells[row, col] = cell;
                }
            }

            // Цифры
            for (int num = 8, r = 1; num != 0; num--, r++)
            {
                var leftText = CreateText(num.ToString());

                leftText.Foreground = (r + 0) % 2 == 0 ? Brushes.Beige : Brushes.SaddleBrown;
                leftText.HorizontalAlignment = HorizontalAlignment.Left;
                leftText.VerticalAlignment = VerticalAlignment.Top;
                leftText.Margin = new Thickness(3, 0, 0, 0);
                Grid.SetRow(leftText, r);
                Grid.SetColumn(leftText, 0);
                BoardGrid.Children.Add(leftText);
            }

            char letter = 'A';
            for (int col = 0; col <= 8; col++)
            {
                var topText = CreateText(letter.ToString());

                topText.Foreground = (col + 8) % 2 == 0 ? Brushes.Beige : Brushes.SaddleBrown;
                topText.HorizontalAlignment = HorizontalAlignment.Right;
                topText.VerticalAlignment = VerticalAlignment.Bottom;
                topText.Margin = new Thickness(0, 0, 3, 0);
                Grid.SetRow(topText, 8);
                Grid.SetColumn(topText, col);
                BoardGrid.Children.Add(topText);
                letter++;
            }


            blackTimerLabel = new Label()
            {
                Name = "lblBlackTimer",
                Content = "0:00",
                FontSize = 30,
                Width = 100,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                Foreground = new SolidColorBrush(Color.FromRgb(179, 179, 179)),
                Background = new SolidColorBrush(Color.FromRgb(42, 42, 42))
            };
            Grid.SetRow(blackTimerLabel, 0);
            Grid.SetColumn(blackTimerLabel, 7);
            BoardGrid.Children.Add(blackTimerLabel);

            whiteTimerLabel = new Label()
            {
                Name = "lblWhiteTimer",
                Content = "0:00",
                FontSize = 30,
                Width = 100,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                Foreground = Brushes.Black,
                Background = Brushes.White
            };
            Grid.SetRow(whiteTimerLabel, 9);
            Grid.SetColumn(whiteTimerLabel, 7);
            BoardGrid.Children.Add(whiteTimerLabel);


        }

        private void OnTimeChanged(TimeSpan white, TimeSpan black)
        {
            whiteTimerLabel.Content = white.ToString(@"mm\:ss");
            blackTimerLabel.Content = black.ToString(@"mm\:ss");
        }

        private void OnTimeExpired(PieceColor color)
        {
            EndGame($"{color} проиграл по времени");
        }


        private TextBlock CreateText(string text) => new TextBlock
        {
            FontSize = 18,
            FontWeight = FontWeights.Bold,
            Text = text
        };

        private void SetupPieces()
        {
            //// Чёрные
            //board[0, 0] = new Rook(PieceColor.Black);
            //board[0, 1] = new Knight(PieceColor.Black);
            //board[0, 2] = new Bishop(PieceColor.Black);
            //board[0, 3] = new Queen(PieceColor.Black);
            //board[0, 4] = new King(PieceColor.Black);
            //board[0, 5] = new Bishop(PieceColor.Black);
            //board[0, 6] = new Knight(PieceColor.Black);
            ////board[0, 7] = new Rook(PieceColor.Black);
            //for (int col = 0; col < 6; col++)
            //    board[1, col] = new Pawn(PieceColor.Black);
            //board[1, 7] = new Pawn(PieceColor.White);

            //// Белые
            //board[7, 0] = new Rook(PieceColor.White);
            //board[7, 1] = new Knight(PieceColor.White);
            //board[7, 2] = new Bishop(PieceColor.White);
            //board[7, 3] = new Queen(PieceColor.White);
            //board[7, 4] = new King(PieceColor.White);
            //board[7, 5] = new Bishop(PieceColor.White);
            ////board[7, 6] = new Knight(PieceColor.White);
            ////board[7, 7] = new Rook(PieceColor.White);
            //for (int col = 0; col < 6; col++)
            //    board[6, col] = new Pawn(PieceColor.White);
            //board[6, 7] = new Pawn(PieceColor.Black);

            // Чёрные
            board[0, 0] = new Rook(PieceColor.Black);
            board[0, 1] = new Knight(PieceColor.Black);
            board[0, 2] = new Bishop(PieceColor.Black);
            board[0, 3] = new Queen(PieceColor.Black);
            board[0, 4] = new King(PieceColor.Black);
            board[0, 5] = new Bishop(PieceColor.Black);
            //board[0, 6] = new Knight(PieceColor.Black);
            //board[0, 7] = new Rook(PieceColor.Black);
            for (int col = 0; col < 6; col++)
                board[1, col] = new Pawn(PieceColor.Black);
            board[1, 7] = new Queen(PieceColor.White);

            // Белые
            board[7, 0] = new Rook(PieceColor.White);
            board[7, 1] = new Knight(PieceColor.White);
            board[7, 2] = new Bishop(PieceColor.White);
            board[3, 7] = new Queen(PieceColor.White);
            board[7, 4] = new King(PieceColor.White);
            board[7, 5] = new Bishop(PieceColor.White);
            //board[7, 6] = new Knight(PieceColor.White);
            //board[7, 7] = new Rook(PieceColor.White);
            for (int col = 0; col < 6; col++)
                board[6, col] = new Pawn(PieceColor.White);
            //board[6, 7] = new Queen(PieceColor.Black);


            _whiteKingPos = FindKingPosition(PieceColor.White);
            _blackKingPos = FindKingPosition(PieceColor.Black);
        }

        private (int row, int col) FindKingPosition(PieceColor color)
        {
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    if (board[r, c]?.Type == PieceType.King && board[r, c]?.Color == color)
                        return (r, c);
            // если не нашли — возвращаем -1,-1 (caller должен обрабатывать)
            return (-1, -1);
        }

        public void RenderPieces()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var cell = cells[row, col];
                    cell.Child = null;

                    var piece = board[row, col];
                    if (piece == null) continue;

                    cell.Child = new Image
                    {
                        Source = new BitmapImage(new Uri(piece.ImagePath, UriKind.Relative)),
                        Stretch = Stretch.Uniform,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                }
            }

            // после прорисовки можно обновить подсветку шаха (если есть)
            UpdateCheckHighlight();
        }

        private async void Cell_Click(object sender, MouseButtonEventArgs e)
        {
            if (_gameOver) return;

            var cell = (Border)sender;
            var (row, col) = ((int, int))cell.Tag;
            var clickedPiece = board[row, col];


            if (_selectedCell == null)
            {
                if (clickedPiece == null || clickedPiece.Color != walkingСolor) return;

                _selectedCell = (row, col);
                HighlightMoves(row, col);
                return;
            }


            var (fromRow, fromCol) = _selectedCell.Value;
            var selectedPiece = board[fromRow, fromCol];
            if (clickedPiece != null && clickedPiece.Color == walkingСolor)
            {
                _selectedCell = (row, col);
                ClearHighlights();
                HighlightMoves(row, col);
                return;
            }


            bool moved = TryMovePiece(fromRow, fromCol, row, col);
            if (moved)
            {
                if (!_clockStarted)
                {
                    clock.Start();
                    _clockStarted = true;
                }
                ChangeOfStroke();

                CheckGameStateAfterMove();
            }

            _selectedCell = null;
            if (!moved) ClearHighlights();

            if (_ai != null && walkingСolor == _ai.aiColor)
            {
                await MakeAiMoveAsync();
                CheckGameStateAfterMove();
            }

        }
        private async Task MakeAiMoveAsync()
        {
            await Task.Delay(600);

            var move = _ai.GetBestMove();
            TryMovePiece(move.FromRow, move.FromCol, move.ToRow, move.ToCol);
            ChangeOfStroke();
        }
        private void CheckGameStateAfterMove()
        {
            var opponent = walkingСolor;

            if (IsKingInCheck(opponent))
            {
                UpdateCheckHighlight();

                if (IsCheckmate(opponent))
                    EndGame($"{opponent} получил мат");
            }
            else
            {
                if (IsStalemate(opponent))
                    EndGame("Пат — ничья");
                else
                    ClearHighlights();
            }
        }
        private bool HasAnyLegalMove(PieceColor color)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    var piece = board[r, c];
                    if (piece == null || piece.Color != color)
                        continue;

                    foreach (var (toRow, toCol) in piece.GetPossibleMoves(board, r, c))
                    {
                        if (IsMoveLegal(r, c, toRow, toCol))
                            return true;
                    }
                }
            }
            return false;
        }

        private bool IsStalemate(PieceColor color)
        {
            return !IsKingInCheck(color) && !HasAnyLegalMove(color);
        }

        private void ChangeOfStroke()
        {
            clock.SwitchTurn();
            walkingСolor = clock.ActiveColor;
            UpdateTimerHighlight();
        }

        private void UpdateTimerHighlight()
        {
            blackTimerLabel.Background = new SolidColorBrush(Color.FromRgb(42, 42, 42));
            blackTimerLabel.Foreground = new SolidColorBrush(Color.FromRgb(179, 179, 179));
            whiteTimerLabel.Background = new SolidColorBrush(Color.FromRgb(42, 42, 42));
            whiteTimerLabel.Foreground = new SolidColorBrush(Color.FromRgb(179, 179, 179));

            if (clock.ActiveColor == PieceColor.White)
            {
                whiteTimerLabel.Background = Brushes.White;
                whiteTimerLabel.Foreground = Brushes.Black;
            }
            else
            {
                blackTimerLabel.Background = Brushes.White;
                blackTimerLabel.Foreground = Brushes.Black;
            }
        }

        private void HighlightMoves(int row, int col)
        {
            ClearHighlights();

            var piece = board[row, col];
            if (piece == null) return;

            // показываем только легальные ходы
            var possible = piece.GetPossibleMoves(board, row, col);
            foreach (var move in possible)
            {
                if (IsMoveLegal(row, col, move.row, move.col))
                    cells[move.row, move.col].Background = Brushes.LightGreen;
            }

            cells[row, col].Background = Brushes.Yellow;
        }

        private void ClearHighlights()
        {
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    cells[r, c].Background = (r + c) % 2 == 0 ? Brushes.Beige : Brushes.SaddleBrown;
        }

        private bool TryMovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            var piece = board[fromRow, fromCol];
            if (piece == null) return false;

            var rawMoves = piece.GetPossibleMoves(board, fromRow, fromCol);
            if (!rawMoves.Contains((toRow, toCol))) return false;

            bool shortCastle;
            if (piece.Type == PieceType.King)
            {

                if (toCol == fromCol + 2)
                {
                    if (CanCastle(piece.Color, fromRow, fromCol, toCol))
                    {
                        shortCastle = true;
                        PerformCastle(piece.Color, shortCastle);
                        RenderPieces();
                        return true;
                    }
                    return false;
                }

                if (toCol == fromCol - 2)
                {
                    if (CanCastle(piece.Color, fromRow, fromCol, toCol))
                    {
                        shortCastle = false;
                        PerformCastle(piece.Color, shortCastle);
                        RenderPieces();
                        return true;
                    }
                    return false;
                }
            }


            if (!IsMoveLegal(fromRow, fromCol, toRow, toCol)) return false;


            var captured = board[toRow, toCol];
            board[toRow, toCol] = piece;
            board[fromRow, fromCol] = null;

            if (piece.Type == PieceType.Pawn)
            {
                if ((piece.Color == PieceColor.White && toRow == 0) ||
                    (piece.Color == PieceColor.Black && toRow == 7))
                {
                    if (_ai != null && piece.Color == _ai.aiColor)
                    {
                        board[toRow, toCol] = new Queen(piece.Color);
                    }
                    else
                    {
                        Border cell = cells[toRow, toCol];
                        Point screenPoint = cell.PointToScreen(new Point(0, 0));
                        PromWindBase wnd = piece.Color == PieceColor.White
                            ? new PromotionWindWhite()
                            : new PromotionWindBlack();

                        wnd.Left = screenPoint.X + cell.ActualWidth;
                        wnd.Top = screenPoint.Y;
                        string? result = null;

                        if (wnd.ShowDialog() == true && !string.IsNullOrEmpty(wnd.SelectedFigure))
                        {
                            PromotePawn(wnd.SelectedFigure, toRow, toCol, piece.Color);
                        }
                    }
                }
            }



            if (piece.Type == PieceType.King)
            {
                if (piece.Color == PieceColor.White) _whiteKingPos = (toRow, toCol);
                else _blackKingPos = (toRow, toCol);
                ((King)piece).HasMoved = true;
            }

            if (piece.Type == PieceType.Rook)
                ((Rook)piece).HasMoved = true;


            RenderPieces();
            return true;
        }

        private void PromotePawn(string result, int toRow, int toCol, PieceColor color)
        {
            switch (result)
            {
                case "Queen": board[toRow, toCol] = new Queen(color); break;
                case "Rook": board[toRow, toCol] = new Rook(color); break;
                case "Bishop": board[toRow, toCol] = new Bishop(color); break;
                case "Knight": board[toRow, toCol] = new Knight(color); break;
            }
        }

        private void PerformCastle(PieceColor color, bool shortCastle)
        {
            int row = (color == PieceColor.White) ? 7 : 0;
            if (shortCastle)
            {
                // Король E → G
                board[row, 6] = board[row, 4];
                board[row, 4] = null;

                // Ладья H → F
                board[row, 5] = board[row, 7];
                board[row, 7] = null;

                ((King)board[row, 6]).HasMoved = true;
                ((Rook)board[row, 5]).HasMoved = true;

                if (color == PieceColor.White) _whiteKingPos = (row, 6);
                else _blackKingPos = (row, 6);
            }
            else
            {
                // Король E → C
                board[row, 2] = board[row, 4];
                board[row, 4] = null;

                // Ладья A → D
                board[row, 3] = board[row, 0];
                board[row, 0] = null;

                ((King)board[row, 2]).HasMoved = true;
                ((Rook)board[row, 3]).HasMoved = true;

                if (color == PieceColor.White) _whiteKingPos = (row, 2);
                else _blackKingPos = (row, 2);
            }
        }
        private bool CanCastle(PieceColor color, int fromRow, int fromCol, int toCol)
        {
            var king = board[fromRow, fromCol] as King;
            if (king == null || king.HasMoved)
                return false;

            // Нельзя рокироваться, если король под шахом
            if (IsKingInCheck(color))
                return false;

            bool isShortCastle = (toCol == 6);
            int rookCol = isShortCastle ? 7 : 0;

            var rook = board[fromRow, rookCol] as Rook;
            if (rook == null || rook.HasMoved)
                return false;

            int direction = isShortCastle ? 1 : -1;

            // Проверка, что между королём и ладьёй нет фигур
            for (int c = fromCol + direction; c != rookCol; c += direction)
                if (board[fromRow, c] != null)
                    return false;

            // Клетки, через которые проходит король — не должны быть под боем
            // Король проверяет клетки: E -> F -> G (или E -> D -> C)
            for (int c = fromCol; c != toCol + direction; c += direction)
                if (IsSquareAttacked(fromRow, c, color))
                    return false;

            return true;
        }

        private bool IsMoveLegal(int fromRow, int fromCol, int toRow, int toCol)
        {
            var piece = board[fromRow, fromCol];
            if (piece == null) return false;

            var target = board[toRow, toCol];
            if (target != null && target.Type == PieceType.King)
                return false;

            var backupTarget = target;
            var backupFrom = piece;

            board[toRow, toCol] = piece;
            board[fromRow, fromCol] = null;

            (int, int) oldKingPos = (-1, -1);
            bool kingMoved = false;

            if (piece.Type == PieceType.King)
            {
                kingMoved = true;
                if (piece.Color == PieceColor.White)
                {
                    oldKingPos = _whiteKingPos;
                    _whiteKingPos = (toRow, toCol);
                }
                else
                {
                    oldKingPos = _blackKingPos;
                    _blackKingPos = (toRow, toCol);
                }
            }

            bool legal = !IsKingInCheck(piece.Color);

            board[fromRow, fromCol] = backupFrom;
            board[toRow, toCol] = backupTarget;

            if (kingMoved)
            {
                if (piece.Color == PieceColor.White)
                    _whiteKingPos = oldKingPos;
                else
                    _blackKingPos = oldKingPos;
            }

            return legal;
        }


        public bool IsKingInCheck(PieceColor kingColor)
        {
            var (kingRow, kingCol) = kingColor == PieceColor.White ? _whiteKingPos : _blackKingPos;
            if (kingRow < 0 || kingCol < 0) return false;

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    var piece = board[r, c];
                    if (piece == null || piece.Color == kingColor) continue;

                    var moves = piece.GetPossibleMoves(board, r, c);
                    if (moves.Contains((kingRow, kingCol))) return true;
                }
            }

            return false;
        }

        private void UpdateCheckHighlight()
        {
            // сбрасываем
            ClearHighlights();

            // подсветка короля, если он под шахом
            if (IsKingInCheck(PieceColor.White))
            {
                var (r, c) = _whiteKingPos;
                if (r >= 0 && c >= 0) cells[r, c].Background = Brushes.Red;
            }
            if (IsKingInCheck(PieceColor.Black))
            {
                var (r, c) = _blackKingPos;
                if (r >= 0 && c >= 0) cells[r, c].Background = Brushes.Red;
            }
        }

        private bool IsCheckmate(PieceColor kingColor)
        {
            if (!IsKingInCheck(kingColor)) return false;

            var (kRow, kCol) = kingColor == PieceColor.White ? _whiteKingPos : _blackKingPos;
            var king = board[kRow, kCol];
            if (king != null)
            {
                foreach (var mv in king.GetPossibleMoves(board, kRow, kCol))
                {
                    if (IsMoveLegal(kRow, kCol, mv.row, mv.col)) return false;
                }
            }

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    var piece = board[r, c];
                    if (piece == null || piece.Color != kingColor) continue;
                    if (piece.Type == PieceType.King) continue;

                    foreach (var mv in piece.GetPossibleMoves(board, r, c))
                    {
                        if (IsMoveLegal(r, c, mv.row, mv.col)) return false;
                    }
                }
            }

            return true;
        }
        public bool IsSquareAttacked(int row, int col, PieceColor color)
        {
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                {
                    var piece = board[r, c];
                    if (piece == null || piece.Color == color) continue;
                    if (piece.GetPossibleMoves(board, r, c).Contains((row, col)))
                        return true;
                }
            return false;
        }

        public PieceColor Opponent(PieceColor c)
    => c == PieceColor.White ? PieceColor.Black : PieceColor.White;

        public Piece? MakeTempMove(Move move) 
        { 
            var captured = board[move.ToRow, move.ToCol]; 
            board[move.ToRow, move.ToCol] = board[move.FromRow, move.FromCol];
            board[move.FromRow, move.FromCol] = null;
            return captured; 
        }

        public void UndoTempMove(Move move, Piece? captured) 
        { 
            board[move.FromRow, move.FromCol] = board[move.ToRow, move.ToCol];
            board[move.ToRow, move.ToCol] = captured; 
        }


        public List<Move> GetAllLegalMoves(PieceColor color)
        {
            var moves = new List<Move>();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var piece = board[row, col];

                    if (piece == null || piece.Color != color)
                        continue;

                    var possibleMoves = piece.GetPossibleMoves(board, row, col);

                    foreach (var (toRow, toCol) in possibleMoves)
                    {
                        if (IsMoveLegal(row, col, toRow, toCol))
                        {
                            moves.Add(new Move(row, col, toRow, toCol));
                        }
                    }
                }
            }

            return moves;
        }
        public void EndGame(string reason)
        {
            if (_gameOver) return;

            _gameOver = true;
            clock.Stop();
            GameEnded?.Invoke(reason);
        }

        internal Piece?[,] GetBoard()
        {
            return board;
        }
    }
}
