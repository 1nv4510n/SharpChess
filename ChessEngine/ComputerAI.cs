using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChessEngine.Pieces;
using static ChessEngine.Enum;
using static ChessEngine.ChessStruct;

namespace ChessEngine
{
    public class ComputerAI
    {
        private readonly ChessGame game;
        private readonly Board board;
        private readonly Random random = new();

        static List<List<double>> pawnEvalWhite = new List<List<double>>() {
            new () { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            new () { 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0 },
            new () {1.0, 1.0, 2.0, 3.0, 3.0, 2.0, 1.0, 1.0 },
            new () {0.5, 0.5, 1.0, 2.5, 2.5, 1.0, 0.5, 0.5 },
            new () {0.0, 0.0, 0.0, 2.0, 2.0, 0.0, 0.0, 0.0 },
            new () {0.5, -0.5, -1.0, 0.0, 0.0, -1.0, -0.5, 0.5 },
            new () {0.5, 1.0, 1.0, -2.0, -2.0, 1.0, 1.0, 0.5 },
            new () { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }
        };
        static List<List<double>> pawnEvalBlack = pawnEvalWhite.AsEnumerable().Reverse().ToList();

        static List<List<double>> knightEval = new List<List<double>>() {
            new() {-5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0 },
            new() {-4.0, -2.0, 0.0, 0.0, 0.0, 0.0, -2.0, -4.0 },
            new() {-3.0, 0.0, 1.0, 1.5, 1.5, 1.0, 0.0, -3.0 },
            new() {-3.0, 0.5, 1.5, 2.0, 2.0, 1.5, 0.5, -3.0 },
            new() {-3.0, 0.0, 1.5, 2.0, 2.0, 1.5, 0.0, -3.0 },
            new() {-3.0, 0.5, 1.0, 1.5, 1.5, 1.0, 0.5, -3.0 },
            new() {-4.0, -2.0, 0.0, 0.5, 0.5, 0.0, -2.0, -4.0 },
            new() { -5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0 }
        };

        static List<List<double>> bishopEvalWhite = new List<List<double>>() {
            new() {-2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0 },
            new() {-1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -1.0 },
            new() {-1.0, 0.0, 0.5, 1.0, 1.0, 0.5, 0.0, -1.0 },
            new() {-1.0, 0.5, 0.5, 1.0, 1.0, 0.5, 0.5, -1.0 },
            new() {-1.0, 0.0, 1.0, 1.0, 1.0, 1.0, 0.0, -1.0 },
            new() {-1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, -1.0 },
            new() {-1.0, 0.5, 0.0, 0.0, 0.0, 0.0, 0.5, -1.0 },
            new() {-2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0 }
        };
        static List<List<double>> bishopEvalBlack = bishopEvalWhite.AsEnumerable().Reverse().ToList();

        static List<List<double>> rookEvalWhite = new List<List<double>>() {
            new() {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            new() {0.5, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 0.5 },
            new() {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            new() {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            new() {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            new() {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5},
            new() {-0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5 },
            new() { 0.0, 0.0, 0.0, 0.5, 0.5, 0.0, 0.0, 0.0 }
        };
        static List<List<double>> rookEvalBlack = rookEvalWhite.AsEnumerable().Reverse().ToList();

        static List<List<double>> evalQueen = new List<List<double>>() {
            new() {-2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0 },
            new() {-1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -1.0 },
            new() {-1.0, 0.0, 0.5, 0.5, 0.5, 0.5, 0.0, -1.0 },
            new() {-0.5, 0.0, 0.5, 0.5, 0.5, 0.5, 0.0, -0.5 },
            new() {0.0, 0.0, 0.5, 0.5, 0.5, 0.5, 0.0, -0.5 },
            new() {-1.0, 0.5, 0.5, 0.5, 0.5, 0.5, 0.0, -1.0 },
            new() {-1.0, 0.0, 0.5, 0.0, 0.0, 0.0, 0.0, -1.0 },
            new() { -2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0 }
        };

        static List<List<double>> kingEvalWhite = new List<List<double>>() {
            new() {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},
            new() {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},
            new() {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},
            new() {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},
            new() {-2.0, -3.0, -3.0, -4.0, -4.0, -3.0, -3.0, -2.0},
            new() {-1.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -1.0},
            new() {2.0, 2.0, 0.0, 0.0, 0.0, 0.0, 2.0, 2.0},
            new() {2.0, 3.0, 1.0, 0.0, 0.0, 1.0, 3.0, 2.0}
        };
        static List<List<double>> kingEvalBlack = kingEvalWhite.AsEnumerable().Reverse().ToList();

        public ComputerAI(ChessGame game)
        {
            this.game = game;
            this.board = game.board;
        }

        private double GetPieceValue(Piece? piece)
        {
            if (piece is null) return 0;

            double value = 0;

            switch (piece.name)
            {
                case PieceNames.PAWN:
                    value = 10 + (piece.color == Colors.WHITE ? pawnEvalWhite[piece.cell.y][piece.cell.x] : pawnEvalBlack[piece.cell.y][piece.cell.x]);
                    break;
                case PieceNames.ROOK:
                    value = 50 + (piece.color == Colors.WHITE ? rookEvalWhite[piece.cell.y][piece.cell.x] : rookEvalBlack[piece.cell.y][piece.cell.x]);
                    break;
                case PieceNames.KNIGHT:
                    value = 30 + knightEval[piece.cell.y][piece.cell.x];
                    break;
                case PieceNames.BISHOP:
                    value = 35 + (piece.color == Colors.WHITE ? bishopEvalWhite[piece.cell.y][piece.cell.x] : bishopEvalBlack[piece.cell.y][piece.cell.x]);
                    break;
                case PieceNames.QUEEN:
                    value = 90 + evalQueen[piece.cell.y][piece.cell.x];
                    break;
                case PieceNames.KING:
                    value = 900 + (piece.color == Colors.WHITE ? kingEvalWhite[piece.cell.y][piece.cell.x] : kingEvalBlack[piece.cell.y][piece.cell.x]);
                    break;
            }
            return piece.color == Colors.WHITE ? value : -value;
        }

        private double EvaluateBoard()
        {
            double totalEvaluation = 0;

            foreach (var row in board.cells)
            {
                foreach (var cell in row)
                {
                    totalEvaluation += GetPieceValue(cell.piece);
                }
            }
            return totalEvaluation;
        }

        private double Minimax(Colors color, int depth, double alpha, double beta, bool isMaximisingPlayer)
        {
            if (depth == 0)
            {
                return -EvaluateBoard();
            }

            var gameMoves = board.GetUglyMoves(color);

            if (isMaximisingPlayer)
            {
                double bestMove = -9999;
                foreach (var move in gameMoves)
                {
                    game.MovePiece(move.SourceCell, move.TargetCell, AITest: true);
                    bestMove = Math.Max(bestMove, Minimax(color, depth - 1, alpha, beta, !isMaximisingPlayer));
                    game.UndoMove();
                    alpha = Math.Max(alpha, bestMove);
                    if (beta <= alpha) return bestMove;
                }
                return bestMove;
            } else
            {
                double bestMove = 9999;
                foreach (var move in gameMoves)
                {
                    game.MovePiece(move.SourceCell, move.TargetCell, AITest: true);
                    bestMove = Math.Min(bestMove, Minimax(color, depth - 1, alpha, beta, !isMaximisingPlayer));
                    game.UndoMove();
                    beta = Math.Min(beta, bestMove);
                    if (beta <= alpha) return bestMove;
                }
                return bestMove;
            }
        }

        private Move MinimaxRoot(Colors color, int depth, bool isMaximisingPlayer)
        {
            var gameMoves = board.GetUglyMoves(color);
            double bestValue = -9999;
            Move bestMove = gameMoves[random.Next(gameMoves.Count)];

            foreach (var move in gameMoves)
            {

                game.MovePiece(move.SourceCell, move.TargetCell, AITest: true);
                double value = Minimax(color, depth - 1, -10000, 10000, !isMaximisingPlayer);
                game.UndoMove();

                if (value > bestValue)
                {
                    bestValue = value;
                    bestMove = move;
                }
            }
            return bestMove;
        }

        public Move GetBestMove(Colors color)
        {
            int depth = 3;

            if (this.board.KingIsUnderCheck(color).Any())
            {
                var kingEscapeMoves = this.board.KingEscapeMoves(color);

                var randomMove = kingEscapeMoves[random.Next(kingEscapeMoves.Count)];
                Move escapeMove = new Move(color, randomMove[0], randomMove[random.Next(1, randomMove.Count)]);
                return escapeMove;
            }
            return MinimaxRoot(color, depth, true);
        }
    }
}