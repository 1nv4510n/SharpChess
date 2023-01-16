using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Pieces;

using static ChessEngine.Enum;
using static ChessEngine.ChessStruct;

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
                _ = new Pawn(this.GetCell(i, 1), Colors.BLACK);
                _ = new Pawn(this.GetCell(i, 6), Colors.WHITE);
            }
        }

        private void AddRooks()
        {
            _ = new Rook(this.GetCell(0, 0), Colors.BLACK);
            _ = new Rook(this.GetCell(7, 0), Colors.BLACK);
            _ = new Rook(this.GetCell(0, 7), Colors.WHITE);
            _ = new Rook(this.GetCell(7, 7), Colors.WHITE);
        }

        private void AddKnights()
        {
            _ = new Knight(this.GetCell(1, 0), Colors.BLACK);
            _ = new Knight(this.GetCell(6, 0), Colors.BLACK);
            _ = new Knight(this.GetCell(1, 7), Colors.WHITE);
            _ = new Knight(this.GetCell(6, 7), Colors.WHITE);
        }

        private void AddBishops()
        {
            _ = new Bishop(this.GetCell(2, 0), Colors.BLACK);
            _ = new Bishop(this.GetCell(5, 0), Colors.BLACK);
            _ = new Bishop(this.GetCell(2, 7), Colors.WHITE);
            _ = new Bishop(this.GetCell(5, 7), Colors.WHITE);
        }

        private void AddQueens()
        {
            _ = new Queen(this.GetCell(3, 0), Colors.BLACK);
            _ = new Queen(this.GetCell(3, 7), Colors.WHITE);
        }
        private void AddKings()
        {
            _ = new King(this.GetCell(4, 0), Colors.BLACK);
            _ = new King(this.GetCell(4, 7), Colors.WHITE);
        }

        public void AddPieces()
        {
            AddPawns();
            AddRooks();
            AddKnights();
            AddQueens();
            AddKings();
            AddBishops();
        }

        public void InitTestPlacement()
        {
            _ = new King(this.GetCellFromPgn("c1"), Colors.BLACK);
            _ = new King(this.GetCellFromPgn("b3"), Colors.WHITE);
            _ = new Queen(this.GetCellFromPgn("c5"), Colors.BLACK);
            _ = new Rook(this.GetCellFromPgn("g4"), Colors.BLACK);
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

        public List<Cell> HighlightMoves(Cell selectedCell)
        {
            List<Cell> highlighted = new();

            if (selectedCell.piece is not null)
            {
                if (selectedCell.piece.name == PieceNames.KING)
                {
                    foreach (var move in selectedCell.piece.GetAttackDirection())
                    {
                        if (selectedCell.piece.CanMove(move))
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
                                highlighted.Add(targetCell);
                            }
                        }
                    }
                }
            }

            return highlighted;
        }

        public List<List<Cell>> GetAllMoves(Colors color)
        {
            List<List<Cell>> moves = new();

            var kingEscapeMoves = KingEscapeMoves(color);
            if (kingEscapeMoves.Any())
            {
                return kingEscapeMoves;
            }

            foreach (var row in cells)
            {
                foreach (var targetCell in row)
                {
                    List<Cell> move = new();

                    if (targetCell.piece?.color == color)
                    {
                        var targetCellMoves = HighlightMoves(targetCell);
                        if (targetCellMoves.Any())
                        {
                            move.Add(targetCell);
                            move.AddRange(targetCellMoves);
                            moves.Add(move);
                        }
                    }
                } 
            }
            
            return moves;
        }

        public List<Move> GetUglyMoves(Colors color)
        {
            List<Move> moves = new();
            foreach (var row in cells)
            {
                foreach (var sourceCell in row)
                {
                    if (sourceCell.piece?.color == color)
                    {
                        foreach (var targetCellMove in HighlightMoves(sourceCell))
                        {
                            Move move = new Move(color, sourceCell, targetCellMove);
                            moves.Add(move);
                        }
                    }
                }
            }
            return moves;
        }

        public List<Cell> GetPieces(PieceNames name, Colors color)
        {
            List<Cell> pieceList = new();
            foreach (var row in this.cells)
            {
                foreach (var checkCell in row)
                {
                    if (checkCell.piece?.color == color && checkCell.piece?.name == name)
                    {
                        pieceList.Add(checkCell);
                    }
                }
            }
            return pieceList;
        }

        public List<Cell> KingIsUnderCheck(Colors color)
        {
            List<Cell> checkPieces = new();
            Cell king = GetPieces(PieceNames.KING, color)[0];
            Colors oppositeColor = color == Colors.WHITE ? Colors.BLACK : Colors.WHITE;
            
            foreach (var row in this.cells)
            {
                foreach (var checkCell in row)
                {
                    if (checkCell.piece?.color == oppositeColor)
                    {
                        if (checkCell.piece.name == PieceNames.KNIGHT)
                        {
                            int dx = Math.Abs(checkCell.x - king.x);
                            int dy = Math.Abs(checkCell.y - king.y);
                            if ((dx == 1 && dy == 2) || (dx == 2 && dy == 1))
                            {
                                checkPieces.Add(checkCell);
                                continue;
                            }
                        }
                        if (checkCell.piece.name == PieceNames.PAWN)
                        {
                            if (checkCell.piece.GetAttackDirection().Contains(king))
                            {
                                checkPieces.Add(checkCell);
                                continue;
                            }
                        }
                        if (checkCell.piece.name == PieceNames.QUEEN)
                        {
                            if (king.IsEmptyVertical(checkCell) 
                                || king.IsEmptyHorizontal(checkCell) 
                                || king.IsEmptyDiagonal(checkCell))
                            {
                                checkPieces.Add(checkCell);
                                continue;
                            }
                        }
                        if (checkCell.piece.name == PieceNames.ROOK)
                        {
                            if (king.IsEmptyVertical(checkCell) || king.IsEmptyHorizontal(checkCell))
                            {
                                checkPieces.Add(checkCell);
                                continue;
                            }
                        }
                        if (checkCell.piece.name == PieceNames.BISHOP)
                        {
                            if (king.IsEmptyDiagonal(checkCell))
                            {
                                checkPieces.Add(checkCell);
                                continue;
                            }
                        }
                    }
                }
            }
            return checkPieces;
        }

        public List<List<Cell>> KingEscapeMoves(Colors color)
        {
            Cell king = GetPieces(PieceNames.KING, color).First();
            var checkSource = KingIsUnderCheck(color);
            List<List<Cell>> escapeMoves = new();
            if (checkSource.Any())
            {
                var kingMoves = HighlightMoves(king);
                if (kingMoves.Any())
                {
                    kingMoves.Insert(0, king);
                    escapeMoves.Add(kingMoves);
                }

                if (checkSource.Count == 1)
                {
                    List<Cell> attackLines = new();
                    foreach (var attacker in checkSource)
                    {
                        if (attacker.piece?.name != PieceNames.KING)
                        {
                            if (attacker.piece?.name == PieceNames.PAWN || attacker.piece?.name == PieceNames.KNIGHT)
                            {
                                attackLines.Add(attacker);
                            } else
                            {
                                attackLines.AddRange(king.GetPathToCell(attacker));
                            }
                        }
                    }

                    foreach (var row in this.cells)
                    {
                        foreach (var checkcell in row)
                        {
                            if (checkcell.piece?.color == color && checkcell.piece?.name != PieceNames.KING)
                            {
                                var checkCellMoves = HighlightMoves(checkcell);
                                foreach (var move in checkCellMoves)
                                {
                                    if (attackLines.Contains(move))
                                    {
                                        escapeMoves.Add(new List<Cell> { checkcell, move });
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    return escapeMoves;
                } 
                else
                {
                    return escapeMoves;
                }
            } else
            {
                return escapeMoves;
            }
        }

        public bool IsCheckmate(Colors color)
        {
            return !KingEscapeMoves(color).Any();
        }

        public bool IsStalemate(Colors color)
        {
            int countPieces = 0;
            foreach (var row in cells)
            {
                foreach (var cell in row)
                {
                    if (cell.piece is not null)
                    {
                        countPieces += 1;
                    }
                }
            }
            if (countPieces == 2) return true;
            return (!KingIsUnderCheck(color).Any() && !GetAllMoves(color).Any());
        }
    }
}
