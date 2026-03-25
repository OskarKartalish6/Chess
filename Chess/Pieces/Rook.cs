using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Logic;

namespace Chess.Pieces
{
    class Rook : Piece
    {
        public override string ImagePath => Color == PieceColor.White ? "PhotosOfFigures/Chess_rlt60.png" : "PhotosOfFigures/Chess_rdt60.png"; 
        public bool HasMoved { get; set; } = false;

        public Rook(PieceColor color) : base(color, PieceType.Rook) { }

        public override List<(int row, int col)> GetPossibleMoves(Piece?[,] board, int currentRow, int currentCol)
        {
            var moves = new List<(int, int)>();

            int[] dRows = { 0, 0, -1, 1};
            int[] dCols = { -1, 1, 0, 0};
            for (int i = 0; i < 4; i++)
            {
                int newRow = currentRow + dRows[i];
                int newCol = currentCol + dCols[i];

                while (IsInsideBoard(newRow, newCol))
                {
                    var targetPiece = board[newRow, newCol];

                    if (targetPiece == null)
                    {
                        moves.Add((newRow, newCol));
                    }
                    else if (targetPiece.Color != this.Color)
                    {
                        moves.Add((newRow, newCol));
                        break;
                    }
                    else
                    {
                        break;
                    }

                    newRow += dRows[i];
                    newCol += dCols[i];

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
