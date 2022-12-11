using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;
using static ChessEngine.ChessStruct;

namespace ChessEngine
{
    public class ChessGame
    {
        public Board board;
        private Player player1;
        private Player player2;
        
        public Player currentTurn;
        public Move? previousTurn = null;
        public bool isGameOver;
        public ChessStatus? endGameStatus;
        private ComputerAI computer;

        public ChessGame(Player player1, Player player2)
        {
            board = new Board();
            board.AddPieces();
            // board.InitTestPlacement();

            this.player1 = player1;
            this.player2 = player2;

            currentTurn = player1.Color == Colors.WHITE ? player1 : player2;
            isGameOver = false;
            endGameStatus = null;
            computer = new ComputerAI(board);
        }

        public List<Cell> GetAllPieces()
        {
            List<Cell> pieces = new();
            foreach (var row in this.board.cells)
            {
                foreach (var cell in row) 
                {
                    if (cell.piece is not null)
                    {
                        pieces.Add(cell);
                    }
                }
            }
            return pieces;
        }

        public ChessEvents MovePiece(Cell sourceCell, Cell targetCell)
        {
            bool isTargetEmpty = targetCell.piece is null;
            sourceCell.MovePiece(targetCell);
            previousTurn = new Move(currentTurn.Color, sourceCell, targetCell);
            currentTurn = currentTurn == player2 ? player1 : player2;
            
            if (this.board.KingIsUnderCheck(currentTurn.Color).Any()) {
                this.currentTurn.Status = ChessStatus.CHECK;
                
                if (this.board.IsCheckmate(currentTurn.Color))
                {
                    this.isGameOver = true;
                    this.currentTurn.Status = ChessStatus.LOSE;
                    this.endGameStatus = ChessStatus.CHECKMATE;

                    return ChessEvents.CHECKMATE;
                }

                return ChessEvents.CHECK;
            }

            if (this.board.IsStalemate(currentTurn.Color))
            {
                this.isGameOver = true;
                this.currentTurn.Status = ChessStatus.DRAW;
                this.endGameStatus = ChessStatus.STALEMATE;
                return ChessEvents.STALEMATE;
            }

            if (!isTargetEmpty)
            {
                return ChessEvents.TAKE;
            }
            else
            {
                return ChessEvents.MOVE;
            }
        }
        
        public ChessEvents? ComputerMove()
        {
            if (currentTurn.Type == PlayerType.COMPUTER && !isGameOver)
            {
                Move move = computer.GetMove(currentTurn.Color);
                return MovePiece(move.SourceCell, move.TargetCell);
            }
            return null;
        }
    }
}
