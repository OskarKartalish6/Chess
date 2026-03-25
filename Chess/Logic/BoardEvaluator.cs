using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Pieces;

namespace Chess.Logic
{
    public static class BoardEvaluator
    {
        public static int Evaluate(Piece?[,] board, PieceColor aiColor)
        {
            int score = 0;

            foreach (var piece in board)
            {
                if (piece == null) continue;

                int value = piece.Type switch
                {
                    PieceType.Pawn => 100,
                    PieceType.Knight => 320,
                    PieceType.Bishop => 330,
                    PieceType.Rook => 500,
                    PieceType.Queen => 900,
                    PieceType.King => 20000,
                    _ => 0
                };

                score += piece.Color == aiColor ? value : -value;
            }

            return score;
        }
    }
}
