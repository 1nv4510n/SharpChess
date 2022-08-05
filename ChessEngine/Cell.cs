using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChessEngine.Pieces;
using static ChessEngine.Enum;

namespace ChessEngine
{
    public class Cell
    {
        public int x;
        public int y;
        internal readonly Colors color;
        public readonly Board board;
        public Piece piece;

        public Cell(Board board, int x, int y, Colors color, Piece? piece)
        {
            this.x = x;
            this.y = y;
            this.color = color;
            this.board = board;
            this.piece = piece;
        }

        public string ToPgn()
        {
            int size = 7;
            string horizontal = "hgfedcba";
            string vertical = "12345678";
            return $"{horizontal[size - this.x]}{vertical[size - this.y]}";
        }

        internal bool IsEmpty(Colors? invisibleKing = null)
        {
            if (invisibleKing is not null && (this.piece?.color == invisibleKing) && (this.piece?.name == PieceNames.KING))
            {
                return true;
            }
            return (this.piece == null);
        }

        internal bool IsEnemy(Cell targetCell)
        {
            if (targetCell.piece is not null)
            {
                return targetCell.piece.color != this.piece.color;
            }
            return false;
        }

        internal bool IsEmptyVertical(Cell targetCell, Colors? invisibleKing = null)
        {
            if (this.x != targetCell.x) { return false; }

            int minValue = Math.Min(this.y, targetCell.y);
            int maxValue = Math.Max(this.y, targetCell.y);
            for (int y = minValue + 1; y < maxValue; y++)
            {
                if (!this.board.GetCell(this.x, y).IsEmpty(invisibleKing))
                {
                    return false;
                }
            }
            return true;

        }
        internal bool IsEmptyHorizontal(Cell targetCell, Colors? invisibleKing = null)
        {
            if (this.y != targetCell.y) { return false; }

            int minValue = Math.Min(this.x, targetCell.x);
            int maxValue = Math.Max(this.x, targetCell.x);
            for (int x = minValue + 1; x < maxValue; x++)
            {
                if (!this.board.GetCell(x, this.y).IsEmpty(invisibleKing))
                {
                    return false;
                }
            }
            return true;
        }

        internal bool IsEmptyDiagonal(Cell targetCell, Colors? invisibleKing = null)
        {
            int absX = Math.Abs(targetCell.x - this.x);
            int absY = Math.Abs(targetCell.y - this.y);

            if (absX != absY) { return false; }

            int dx = this.x < targetCell.x ? 1 : -1;
            int dy = this.y < targetCell.y ? 1 : -1;

            for (int i = 1; i < absY; i++)
            {
                if (!this.board.GetCell(this.x + dx * i, this.y + dy * i).IsEmpty(invisibleKing))
                {
                    return false;
                }
            }
            return true;
        }

        internal List<string> GetPathToCell(Cell targetCell)
        {
            List<string> path = new List<string>();

            int absX = Math.Abs(targetCell.x - this.x);
            int absY = Math.Abs(targetCell.y - this.y);

            if (this.x == targetCell.x)
            {
                int minYValue = Math.Min(this.y, targetCell.y);
                int maxYValue = Math.Max(this.y, targetCell.y);
                for (int y = minYValue + 1; y < maxYValue; y++)
                {
                    path.Add(this.board.GetCell(this.x, y).ToPgn());
                }
            } 
            else if (this.y == targetCell.y)
            {
                int minXValue = Math.Min(this.x, targetCell.x);
                int maxXValue = Math.Max(this.x, targetCell.x);
                for (int x = minXValue + 1; y < maxXValue; x++)
                {
                    path.Add(this.board.GetCell(x, this.y).ToPgn());
                }
            }
            else if (absX == absY)
            {
                int dx = this.x < targetCell.x ? 1 : -1;
                int dy = this.y < targetCell.y ? 1 : -1;

                for (int i = 0; i < absY + 1; i++)
                {
                    path.Add(this.board.GetCell(this.x + dx * i, this.y + dy * i).ToPgn());
                }
            }

            return path;
        }
        
        internal void SetPiece(Piece piece)
        {
            this.piece = piece;
            this.piece.cell = this;
        }

        public void MovePiece(Cell targetCell)
        {
            if (this.piece is not null && this.piece.CanMove(targetCell))
            {
                targetCell.SetPiece(this.piece);
                this.piece = null;
                targetCell.piece.MovePiece();
            }
        }

        internal bool IsAttacked(Colors oppositeColor)
        {
            Colors myColor = oppositeColor == Colors.BLACK ? Colors.WHITE : Colors.BLACK;
            foreach (var row in this.board.cells)
            {
                foreach (var checkCell in row)
                {
                    if (checkCell.piece?.color == oppositeColor && checkCell != this)
                    {
                        if (checkCell.piece.name == PieceNames.KNIGHT)
                        {
                            int dx = Math.Abs(checkCell.x - this.x);
                            int dy = Math.Abs(checkCell.y - this.y);
                            if ((dx == 1 && dy == 2) || (dx == 2 && dy == 1))
                            {
                                return true;
                            }
                        }

                        if (checkCell.piece.name == PieceNames.PAWN 
                            && checkCell.piece.GetAttackDirection().Contains(this.ToPgn()))
                        {
                            return true;
                        }

                        if (checkCell.piece.name == PieceNames.QUEEN 
                            && (this.IsEmptyDiagonal(checkCell, myColor) 
                            || this.IsEmptyVertical(checkCell, myColor) 
                            || this.IsEmptyHorizontal(checkCell, myColor)))
                        {
                            return true;
                        }

                        if (checkCell.piece.name == PieceNames.ROOK
                            && (this.IsEmptyVertical(checkCell, myColor)
                            || this.IsEmptyHorizontal(checkCell, myColor)))
                        {
                            return true;
                        }

                        if (checkCell.piece.name == PieceNames.BISHOP
                            && this.IsEmptyDiagonal(checkCell, myColor))
                        {
                            return true;
                        }

                        if (checkCell.piece.name == PieceNames.KING
                            && checkCell.piece.GetAttackDirection().Contains(this.ToPgn()))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
