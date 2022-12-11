using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Enum
    {
        public enum Colors { WHITE, BLACK };
        public enum PlayerType { HUMAN, COMPUTER };
        public enum PieceNames
        {
            PIECE,
            KING,
            KNIGHT,
            PAWN,
            QUEEN,
            ROOK,
            BISHOP
        }

        public enum ChessStatus
        {
            CHECK,
            CHECKMATE,
            STALEMATE,
            WIN,
            LOSE,
            DRAW
        }

        public enum ChessEvents
        {
            MOVE,
            TAKE,
            CHECK,
            CHECKMATE,
            STALEMATE
        }

        public enum BoardParam
        {
            BOARD_SIZE = 8,
            CELL_SIZE = 80,
            OFFSET_X = 38,
            OFFSET_Y = 38
        }
    }
}
