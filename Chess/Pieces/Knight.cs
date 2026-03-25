using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Logic;

namespace Chess.Pieces
{
    class Knight : Piece
    {
        public override string ImagePath => Color == PieceColor.White ? "PhotosOfFigures/Chess_nlt60.png" : "PhotosOfFigures/Chess_ndt60.png";
        public Knight(PieceColor color) : base(color, PieceType.Knight) { }

        public override List<(int row, int col)> GetPossibleMoves(Piece?[,] board, int currentRow, int currentCol)
        {
            var moves = new List<(int, int)>();

            int[] dRows = { 1,-1, -2, -2, -1, 1, 2, 2 };
            int[] dCols = { -2, -2, -1, 1, 2, 2, 1, -1 };
            for (int i = 0; i < 8; i++)
            {
                int newRow = currentRow + dRows[i];
                int newCol = currentCol + dCols[i];


                if (IsInsideBoard(newRow, newCol))
                {
                    var targetPiece = board[newRow, newCol];
                    if (targetPiece == null || targetPiece.Color != this.Color)
                        moves.Add((newRow, newCol));
                }

            }
            return moves;
        }
        private bool IsInsideBoard(int row, int col)
        {
            return row >= 0 && row < 8 && col >= 0 && col < 8;
        }
    }
}
