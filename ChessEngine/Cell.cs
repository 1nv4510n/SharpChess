using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChessEngine.Pieces;

namespace ChessEngine
{
    internal class Cell
    {
        private int _x;
        private int _y;
        private readonly Enum.Colors color;
        internal readonly Board board;
        internal Piece piece;

        internal Cell(Board board, int x, int y, Enum.Colors color, Piece piece)
        {
            _x = x;
            _y = y;
            this.color = color;
            this.board = board;
            this.piece = piece;
        }


    }
}
