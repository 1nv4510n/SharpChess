using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;

namespace ChessEngine.Pieces
{
    public class Queen : Piece
    {
        private const string whiteLogo = @"\logo\white_queen.png";
        private const string blackLogo = @"\logo\black_queen.png";

        public Queen(Cell cell, Colors color) : base(cell, color)
        {
            this.logo = this.color == Colors.BLACK ? blackLogo : whiteLogo;
            this.name = PieceNames.QUEEN;
        }

        internal override bool CanMove(Cell targetCell, bool supportCheck = false)
        {
            if (!base.CanMove(targetCell, supportCheck))
            {
                return false;
            }

            if (this.cell.IsEmptyDiagonal(targetCell))
            {
                return true;
            }
            if (this.cell.IsEmptyHorizontal(targetCell))
            {
                return true;
            }
            if (this.cell.IsEmptyVertical(targetCell))
            {
                return true;
            }

            return false;
        }
    }
}
