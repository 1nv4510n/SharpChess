using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;

namespace ChessEngine.Pieces
{
    public class Rook : Piece
    {
        private const string whiteLogo = @"\logo\white_rook.png";
        private const string blackLogo = @"\logo\black_rook.png";
        internal bool isFirstStep = true;
        public Rook(Cell cell, Colors color) : base(cell, color)
        {
            this.logo = this.color == Colors.BLACK ? blackLogo : whiteLogo;
            this.name = PieceNames.ROOK;
        }

        internal override bool CanMove(Cell targetCell, bool supportCheck = false)
        {
            if (!base.CanMove(targetCell, supportCheck))
            {
                return false;
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
