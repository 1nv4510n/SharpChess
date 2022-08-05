using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;

namespace ChessEngine.Pieces
{
    public class Piece
    {
        internal Cell cell;
        internal Colors color;
        public string logo;
        internal PieceNames name;

        public Piece(Cell cell, Colors color)
        {
            this.cell = cell;
            this.color = color;
            this.cell.piece = this;
            this.name = PieceNames.PIECE;
            this.logo = null;
        }

        internal virtual bool CanMove(Cell targetCell, bool supportCheck = false)
        {
            if (!supportCheck)
            {
                if (targetCell.piece?.color == this.color)
                {
                    return false;
                }
                if (targetCell.piece?.name == PieceNames.KING)
                {
                    return false;
                }
            }

            return true;
        }

        internal bool IsSupported()
        {
            foreach (var row in this.cell.board.cells)
            {
                foreach (var checkCell in row)
                {
                    if (checkCell.piece is not null
                       && checkCell.piece.color == this.color
                       && checkCell.piece.name != PieceNames.KING
                       && checkCell != this.cell
                       && checkCell.piece.CanMove(this.cell, supportCheck: true)) 
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal virtual List<string> GetAttackDirection()
        {
            return new List<string>();
        }

        internal virtual void MovePiece()
        {
            ;
        }
    }
}
