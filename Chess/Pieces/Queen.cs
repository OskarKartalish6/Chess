using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Logic;

namespace Chess.Pieces
{
    internal class Queen : Piece
    {
        public override string ImagePath => Color == PieceColor.White ? "PhotosOfFigures/Chess_qlt60.png" : "PhotosOfFigures/Chess_qdt60.png";
        public Queen(PieceColor color) : base(color, PieceType.Queen) { }

        public override List<(int row, int col)> GetPossibleMoves(Piece?[,] board, int currentRow, int currentCol)
        {
            var moves = new List<(int, int)>();

            int[] dRows = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dCols = { -1, 0, 1, -1, 1, -1, 0, 1 };
            for (int i = 0; i < 8; i++)
            {
                int newRow = currentRow + dRows[i];
                int newCol = currentCol + dCols[i];

                while(IsInsideBoard(newRow,newCol))
                {
                    var targetPiece = board[newRow,newCol];

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
