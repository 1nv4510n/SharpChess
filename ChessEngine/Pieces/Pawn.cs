using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;

namespace ChessEngine.Pieces
{
    public class Pawn : Piece
    {
        private const string whiteLogo = @"\logo\white_pawn.png";
        private const string blackLogo = @"\logo\black_pawn.png";
        internal bool isFirstStep = true;

        public Pawn(Cell cell, Colors color) : base(cell, color)
        {
            this.logo = color == Colors.BLACK ? blackLogo : whiteLogo;
            this.name = PieceNames.PAWN;
        }

        internal override bool CanMove(Cell targetCell, bool supportCheck = false)
        {
            if (!base.CanMove(targetCell, supportCheck))
            {
                return false;
            }

            int direction = this.color == Colors.BLACK ? 1 : -1;
            int firstStepDirection = direction * 2;

            if ((targetCell.y == this.cell.y + direction || (this.isFirstStep && (targetCell.y == this.cell.y + firstStepDirection && this.cell.IsEmptyVertical(targetCell))))
                && targetCell.x == this.cell.x 
                && this.cell.board.GetCell(targetCell.x, targetCell.y).IsEmpty())
            {
                return true;
            }

            if (targetCell.y == this.cell.y + direction 
                && ((targetCell.x == this.cell.x + 1 || targetCell.x == this.cell.x - 1) && (this.cell.IsEnemy(targetCell))))
            {
                return true;
            }

            return false;
        }

        internal override List<Cell> GetAttackDirection()
        {
            int direction = this.color == Colors.BLACK ? 1 : -1;
            List<Cell> attacks = new();

            var addFunc = (int xOffset) => this.cell.board.GetCell(this.cell.x + xOffset, this.cell.y + direction);
            if (this.cell.x == 0)
            {
                attacks.Add(addFunc(1));
            }
            else if (this.cell.x == 7)
            {
                attacks.Add(addFunc(-1));
            }
            else
            {
                attacks.Add(addFunc(-1));
                attacks.Add(addFunc(1));
            }

            return attacks;
        }

        internal override void MovePiece()
        {
            base.MovePiece();
            this.isFirstStep = false;

            if (this.color == Colors.BLACK && this.cell.y == 7)
            {
                new Queen(this.cell, Colors.BLACK);
            }
            if (this.color == Colors.WHITE && this.cell.y == 0)
            {
                new Queen(this.cell, Colors.WHITE);
            }
        }
    }
}
