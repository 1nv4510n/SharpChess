using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;

namespace ChessEngine.Pieces
{
    public class Knight : Piece
    {
        private const string whiteLogo = @"\logo\white_knight.png";
        private const string blackLogo = @"\logo\black_knight.png";

        public Knight(Cell cell, Colors color) : base(cell, color)
        {
            this.logo = this.color == Colors.BLACK ? blackLogo : whiteLogo;
            this.name = PieceNames.KNIGHT;
        }

        internal override bool CanMove(Cell targetCell, bool supportCheck = false)
        {
            if (!base.CanMove(targetCell, supportCheck))
            {
                return false;
            }
            int dx = Math.Abs(this.cell.x - targetCell.x);
            int dy = Math.Abs(this.cell.y - targetCell.y);

            return ((dx == 1 && dy == 2) || (dx == 2 && dy == 1));
        }
    }
}
