using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;

namespace ChessEngine.Pieces
{
    public class Bishop : Piece
    {
        private const string whiteLogo = @"\logo\white_bishop.png";
        private const string blackLogo = @"\logo\black_bishop.png";
        public Bishop(Cell cell, Colors color) : base(cell, color)
        {
            this.logo = this.color == Colors.BLACK ? blackLogo : whiteLogo;
            this.name = PieceNames.BISHOP;
        }

        internal override bool CanMove(Cell targetCell, bool supportCheck = false)
        {
            if (!base.CanMove(targetCell, supportCheck)) return false;
            if (this.cell.IsEmptyDiagonal(targetCell)) return true;
            return false;
        }
    }
}
