using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Logic;

namespace Chess.Pieces
{
    internal class King : Piece
    {
        public override string ImagePath => Color == PieceColor.White ? "PhotosOfFigures/Chess_klt60.png" : "PhotosOfFigures/Chess_kdt60.png";
        public bool HasMoved { get; set; } = false;

        public King(PieceColor color) : base(color, PieceType.King) { }

        public override List<(int row, int col)> GetPossibleMoves(Piece?[,] board, int currentRow, int currentCol)
        {
            var moves = new List<(int, int)>();

            int[] dRows = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dCols = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newRow = currentRow + dRows[i];
                int newCol = currentCol + dCols[i];

                if (IsInsideBoard(newRow, newCol))
                {
                    var targetPiece = board[newRow, newCol];

                    if (targetPiece == null || targetPiece.Color != this.Color)
                    {
                        moves.Add((newRow, newCol));
                    }
                }
            }
            var king = (King)this;
            if (!king.HasMoved)
            {
                int row = currentRow;

                if (board[row, 5] == null && board[row, 6] == null)
                    if (board[row, 7] is Rook rook && !rook.HasMoved)
                        moves.Add((row, 6));

                if (board[row, 1] == null && board[row, 2] == null && board[row, 3] == null)
                    if (board[row, 0] is Rook rook2 && !rook2.HasMoved)
                        moves.Add((row, 2));
            }
            return moves;
        }
        private bool IsInsideBoard(int row, int col)
        {
            return row >= 0 && row < 8 && col >= 0 && col < 8;
        }
    }
}
