using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;
using ChessEngine.Pieces;

namespace ChessEngine
{
    public class Board
    {
        public List<List<Cell>> cells = new List<List<Cell>>();
        public Board()
        {
            for (int i = 0; i < 8; i++)
            {
                List<Cell> row = new();
                for (int j = 0; j < 8; j++)
                {

                    if ((i + j) % 2 != 0)
                    {
                        row.Add(new Cell(this, j, i, Colors.BLACK, null));
                    }
                    else
                    {
                        row.Add(new Cell(this, j, i, Colors.WHITE, null));
                    }
                }
                cells.Add(row);
            }

        }
        private void AddPawns()
        {
            for (int i = 0; i < 8; i++)
            {
                new Pawn(this.GetCell(i, 1), Colors.BLACK);
                new Pawn(this.GetCell(i, 6), Colors.WHITE);
            }
        }

        private void AddRooks()
        {
            new Rook(this.GetCell(0, 0), Colors.BLACK);
            new Rook(this.GetCell(7, 0), Colors.BLACK);
            new Rook(this.GetCell(0, 7), Colors.WHITE);
            new Rook(this.GetCell(7, 7), Colors.WHITE);
        }

        private void AddKnights()
        {
            new Knight(this.GetCell(1, 0), Colors.BLACK);
            new Knight(this.GetCell(6, 0), Colors.BLACK);
            new Knight(this.GetCell(1, 7), Colors.WHITE);
            new Knight(this.GetCell(6, 7), Colors.WHITE);
        }

        private void AddBishops()
        {
            new Bishop(this.GetCell(2, 0), Colors.BLACK);
            new Bishop(this.GetCell(5, 0), Colors.BLACK);
            new Bishop(this.GetCell(2, 7), Colors.WHITE);
            new Bishop(this.GetCell(5, 7), Colors.WHITE);
        }

        private void AddQueens()
        {
            new Queen(this.GetCell(3, 0), Colors.BLACK);
            new Queen(this.GetCell(3, 7), Colors.WHITE);
        }
        private void AddKings()
        {
            new King(this.GetCell(4, 0), Colors.BLACK);
            new King(this.GetCell(4, 7), Colors.WHITE);
        }

        public void AddPieces()
        {
            this.AddPawns();
            this.AddRooks();
            this.AddKnights();
            this.AddQueens();
            this.AddKings();
            this.AddBishops();
        }

        internal Cell GetCell (int x, int y)
        {
            return cells[y][x];
        }

        public Cell GetCellFromPgn (string pgn)
        {
            string horizontal = "abcdefgh";
            string vertical = "87654321";
            return cells[vertical.IndexOf(pgn[1])][horizontal.IndexOf(pgn[0])];
        }

        public List<string> HighlightMoves(Cell selectedCell)
        {
            List<string> highlighted = new List<string>();

            if (selectedCell.piece is not null)
            {
                if (selectedCell.piece.name == PieceNames.KING)
                {
                    foreach (string move in selectedCell.piece.GetAttackDirection())
                    {
                        if (selectedCell.piece.CanMove(this.GetCellFromPgn(move)))
                        {
                            highlighted.Add(move);
                        }
                    }
                }
                else
                {
                    foreach (var row in cells)
                    {
                        foreach (var targetCell in row)
                        {
                            if (selectedCell.piece.CanMove(targetCell))
                            {
                                highlighted.Add(targetCell.ToPgn());
                            }
                        }
                    }
                }
            }

            return highlighted;
        }

        public List<List<string>> GetAllMoves(Colors color)
        {
            List<List<string>> moves = new List<List<string>>();

            foreach (var row in cells)
            {
                foreach (var targetCell in row)
                {
                    List<string> move = new List<string>();

                    if (targetCell.piece?.color == color)
                    {
                        move.Add(targetCell.ToPgn());
                        move.AddRange(this.HighlightMoves(targetCell));
                    }
                } 
            }

            return moves;
        }
        
    }
}
