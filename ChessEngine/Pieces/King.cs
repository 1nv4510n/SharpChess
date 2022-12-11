using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;

namespace ChessEngine.Pieces
{
    public class King : Piece
    {
        private const string whiteLogo = @"\logo\white_king.png";
        private const string blackLogo = @"\logo\black_king.png";
        internal bool isFirstStep = true;

        public King(Cell cell, Colors color) : base(cell, color)
        {
            this.logo = this.color == Colors.BLACK ? blackLogo : whiteLogo;
            this.name = PieceNames.KING;
        }

        internal void Castling(Cell fromCell, Cell toCell)
        {
            toCell.piece = fromCell.piece;
            toCell.piece.cell = toCell;
            fromCell.piece = null;
        }

        internal override bool CanMove(Cell targetCell, bool supportCheck = false)
        {
            if (!base.CanMove(targetCell, supportCheck))
            {
                return false;
            }

            int dx = Math.Abs(this.cell.x - targetCell.x);
            int dy = Math.Abs(this.cell.y - targetCell.y);
            Colors enemyColor = this.color == Colors.WHITE ? Colors.BLACK : Colors.WHITE;

            if (targetCell.IsAttacked(enemyColor)) return false;
            if (targetCell.piece is not null && targetCell.piece.IsSupported()) return false;
            if ((dx == 1 && dy == 0) || (dx == 0 && dy == 1) || (dx == 1 && dy == 1))
            {
                return true;
            }
            return false;
        }

        internal override List<Cell> GetAttackDirection()
        {
            List<Cell> pos = new();
            foreach (var row in cell.board.cells)
            {
                foreach (var checkCell in row)
                {
                    int dx = Math.Abs(this.cell.x - checkCell.x);
                    int dy = Math.Abs(this.cell.y - checkCell.y);
                    if (dx >= 2 || dy >= 2) continue;
                    if ((dx == 1 && dy == 1) || (dx == 0 && dy == 1) || (dx == 1 && dy == 0))
                    {
                        pos.Add(checkCell);
                    }
                }
            }
            if (isFirstStep)
            {
                if (this.color == Colors.WHITE)
                {
                    pos.Add(cell.board.GetCellFromPgn("c1"));
                    pos.Add(cell.board.GetCellFromPgn("g1"));
                } else
                {
                    pos.Add(cell.board.GetCellFromPgn("c8"));
                    pos.Add(cell.board.GetCellFromPgn("g8"));
                }
            }
            return pos;
        }
    }
}
