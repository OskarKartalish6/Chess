using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Logic;

namespace Chess.Pieces
{
    internal class Pawn : Piece
    {
        public override string ImagePath => Color == PieceColor.White ? "PhotosOfFigures/Chess_plt60.png" : "PhotosOfFigures/Chess_pdt60.png";
        public Pawn(PieceColor color) : base(color, PieceType.Pawn) { }

        public override List<(int row, int col)> GetPossibleMoves(Piece?[,] board, int currentRow, int currentCol)
        {
            var moves = new List<(int, int)>();

            int direction = Color == PieceColor.White ? -1 : 1;

            int oneStepRow = currentRow + direction;
            if (IsInsideBoard(oneStepRow, currentCol) && board[oneStepRow, currentCol] == null)
            {
                moves.Add((oneStepRow, currentCol));

                bool isFirstMove = (Color == PieceColor.White && currentRow == 6) ||
                                   (Color == PieceColor.Black && currentRow == 1);

                int twoStepRow = currentRow + 2 * direction;
                if (isFirstMove && board[twoStepRow, currentCol] == null)
                {
                    moves.Add((twoStepRow, currentCol));
                }
            }

            int[] cols = { currentCol - 1, currentCol + 1 };
            foreach (var col in cols)
            {
                if (IsInsideBoard(oneStepRow, col) &&
                    board[oneStepRow, col] != null &&
                    board[oneStepRow, col]?.Color != this.Color)
                {
                    moves.Add((oneStepRow, col));
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
