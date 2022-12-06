using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;
using static ChessEngine.PlayerStruct;

namespace ChessEngine
{
    public class ChessGame
    {
        public Board board;
        private Player player1;
        private Player player2;
        private readonly ComputerAI? computer;
        
        public Player currentTurn;
        private bool isGameOver;
        private ChessStatus? endGameStatus;

        public ChessGame(Player player1, Player player2)
        {
            board = new Board();
            board.AddPieces();

            this.player1 = player1;
            this.player2 = player2;

            if (player1.Type == PlayerType.COMPUTER)
            {
                computer = new ComputerAI(board, player1.Color);
            }
            else if (player2.Type == PlayerType.COMPUTER)
            {
                computer = new ComputerAI(board, player2.Color);
            }

            currentTurn = player1.Color == Colors.WHITE ? player1 : player2;
            isGameOver = false;
            endGameStatus = null;
        }

        public void MovePiece(Cell? sourceCell, Cell targetCell)
        {
            sourceCell?.MovePiece(targetCell);

            Player previousTurn = currentTurn;
            currentTurn = currentTurn.Color == player2.Color ? player1 : player2;
            
            if (currentTurn.Type == PlayerType.COMPUTER)
            {
                Console.WriteLine("Computer is thinking...");
                computer?.Move();
                currentTurn = currentTurn.Color == player2.Color ? player1 : player2;
            }
        }
    }
}
