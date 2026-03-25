using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Logic;

namespace Chess.Pieces
{

    public abstract class Piece
    {
        public PieceColor Color { get; }
        public PieceType Type { get; }
        public abstract string ImagePath { get; }
        protected Piece(PieceColor color, PieceType type)
        {
            Color = color;
            Type = type;
        }

        public abstract List<(int row, int col)> GetPossibleMoves(Piece?[,] board, int currentRow, int currentCol);
    }
}
