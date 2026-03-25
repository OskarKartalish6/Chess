using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    public class AI
    {
        private readonly GameLogic logic;     // ссылка на игровую логику
        public readonly PieceColor aiColor;  // цвет ИИ
        private readonly int maxDepth;        // глубина поиска (сложность)

        public AI(GameLogic logic, PieceColor aiColor, int depth)
        {
            this.logic = logic;
            this.aiColor = aiColor;
            maxDepth = depth;
        }

        public Move GetBestMove()
        {
            var moves = logic.GetAllLegalMoves(aiColor);
            if (moves.Count == 0)
                return default;

            Move bestMove = moves[0];
            int bestValue = int.MinValue;
            Debug.WriteLine($"AI moves: {moves.Count}");
            foreach (var move in moves)
            {
                var captured = logic.MakeTempMove(move);
                int value = Minimax(maxDepth - 1, false);
                logic.UndoTempMove(move, captured);

                if (value > bestValue || (value == bestValue && Random.Shared.Next(2) == 0))
                {
                    bestValue = value;
                    bestMove = move;
                }
                Debug.WriteLine($"{move} score={value}");

            }

            return bestMove;
        }

        private int Minimax(int depth, bool maximizing)
        {
            if (depth == 0)
                return BoardEvaluator.Evaluate(logic.GetBoard(), aiColor);

            PieceColor color = maximizing ? aiColor : logic.Opponent(aiColor);
            int best = maximizing ? int.MinValue : int.MaxValue;

            foreach (var move in logic.GetAllLegalMoves(color))
            {
                var captured = logic.MakeTempMove(move);
                int score = Minimax(depth - 1, !maximizing);
                logic.UndoTempMove(move, captured);

                best = maximizing
                    ? Math.Max(best, score)
                    : Math.Min(best, score);
            }

            return best;
        }
    }
}

