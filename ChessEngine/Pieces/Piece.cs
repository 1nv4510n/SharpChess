using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;

namespace ChessEngine.Pieces
{
    public abstract class Piece : ICloneable
    {
        internal Cell cell;
        public Colors color;
        public string? logo;
        internal PieceNames name;
        internal bool isFirstStep = true;

        public Piece(Cell cell, Colors color)
        {
            this.cell = cell;
            this.color = color;
            this.cell.piece = this;
            this.name = PieceNames.PIECE;
            this.logo = null;
        }

        public object Clone() => MemberwiseClone();

        internal virtual bool CanMove(Cell targetCell, bool supportCheck = false)
        {
            if (!supportCheck)
            {
                if (targetCell.piece?.color == this.color) return false;
                if (targetCell.piece?.name == PieceNames.KING) return false;
            }

            if (!supportCheck && this.name != PieceNames.KING)
            {
                Cell currentKing = this.cell.board.GetPieces(PieceNames.KING, this.color).First();
                Colors oppositeColor = this.color == Colors.WHITE ? Colors.BLACK : Colors.WHITE;

                var enemyBishops = this.cell.board.GetPieces(PieceNames.BISHOP, oppositeColor);
                var enemyRooks = this.cell.board.GetPieces(PieceNames.ROOK, oppositeColor);
                var enemyQueens = this.cell.board.GetPieces(PieceNames.QUEEN, oppositeColor);

                if (this.cell.IsEmptyHorizontal(currentKing))
                {
                    foreach (var queen in enemyQueens)
                    {
                        if (this.cell.IsEmptyHorizontal(queen))
                        {
                            if ((this.name == PieceNames.QUEEN 
                                || this.name == PieceNames.ROOK) 
                                && !this.cell.GetPathToCell(queen).Contains(targetCell))
                            {
                                return false;
                            }
                            if (this.name == PieceNames.BISHOP
                                || this.name == PieceNames.PAWN
                                || this.name == PieceNames.KNIGHT)
                            {
                                return false;
                            }
                        }
                    }
                    foreach (var rook in enemyRooks)
                    {
                        if (this.cell.IsEmptyHorizontal(rook))
                        {
                            if ((this.name == PieceNames.QUEEN
                                || this.name == PieceNames.ROOK)
                                && !this.cell.GetPathToCell(rook).Contains(targetCell))
                            {
                                return false;
                            }
                            if (this.name == PieceNames.BISHOP
                                || this.name == PieceNames.PAWN
                                || this.name == PieceNames.KNIGHT)
                            {
                                return false;
                            }
                        }
                    }
                }
                if (this.cell.IsEmptyVertical(currentKing))
                {
                    foreach (var queen in enemyQueens)
                    {
                        if (this.cell.IsEmptyVertical(queen))
                        {
                            if ((this.name == PieceNames.QUEEN
                                || this.name == PieceNames.ROOK)
                                && !this.cell.GetPathToCell(queen).Contains(targetCell))
                            {
                                return false;
                            }
                            if (this.name == PieceNames.PAWN) {
                                if (this.GetAttackDirection().Contains(targetCell))
                                {
                                    return false;
                                }
                            }
                            if (this.name == PieceNames.BISHOP
                                || this.name == PieceNames.KNIGHT)
                            {
                                return false;
                            }
                        }
                    }
                    foreach (var rook in enemyRooks)
                    {
                        if (this.cell.IsEmptyVertical(rook))
                        {
                            if ((this.name == PieceNames.QUEEN
                                || this.name == PieceNames.ROOK)
                                && !this.cell.GetPathToCell(rook).Contains(targetCell))
                            {
                                return false;
                            }
                            if (this.name == PieceNames.PAWN)
                            {
                                if (this.GetAttackDirection().Contains(targetCell))
                                {
                                    return false;
                                }
                            }
                            if (this.name == PieceNames.BISHOP
                                || this.name == PieceNames.KNIGHT)
                            {
                                return false;
                            }
                        }
                    }
                }
                if (this.cell.IsEmptyDiagonal(currentKing))
                {
                    foreach (var queen in enemyQueens)
                    {
                        if (this.cell.IsEmptyDiagonal(queen))
                        {
                            if ((this.name == PieceNames.QUEEN
                                || this.name == PieceNames.BISHOP)
                                && !this.cell.GetPathToCell(queen).Contains(targetCell))
                            {
                                return false;
                            }
                            if (this.name == PieceNames.PAWN)
                            {
                                if (targetCell == queen && this.GetAttackDirection().Contains(queen))
                                {
                                    return true;
                                } else
                                {
                                    return false;
                                }
                            }
                            if (this.name == PieceNames.BISHOP
                                || this.name == PieceNames.KNIGHT)
                            {
                                return false;
                            }
                        }
                    }
                    foreach (var bishop in enemyBishops)
                    {
                        if (this.cell.IsEmptyDiagonal(bishop))
                        {
                            if ((this.name == PieceNames.QUEEN
                                || this.name == PieceNames.BISHOP)
                                && !this.cell.GetPathToCell(bishop).Contains(targetCell))
                            {
                                return false;
                            }
                            if (this.name == PieceNames.PAWN)
                            {
                                if (targetCell == bishop && this.GetAttackDirection().Contains(bishop))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            if (this.name == PieceNames.BISHOP
                                || this.name == PieceNames.KNIGHT)
                            {
                                return false;
                            }
                        }
                    }
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

        internal virtual List<Cell> GetAttackDirection()
        {
            return new();
        }

        internal virtual void MovePiece()
        {
            ;
        }
    }
}
