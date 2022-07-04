using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    internal class Enum
    {
        public enum Colors { WHITE, BLACK };
        enum PieceNames
        {
            PIECE,
            KING,
            KNIGHT,
            PAWN,
            QUEEN,
            ROOK,
            BISHOP
        }

        enum ChessStatus
        {
            CHECK,
            CHECKMATE,
            STALEMATE,
            WIN,
            LOSE,
            DRAW
        }
    }
}
