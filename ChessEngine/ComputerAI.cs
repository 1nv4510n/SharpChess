using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;

namespace ChessEngine
{
    class ComputerAI
    {
        private readonly Board board;
        private readonly Colors color;
        private readonly Random random = new();
        public ComputerAI(Board board, Colors color)
        {
            this.board = board;
            this.color = color;
        }
        
        public void Move() {
            List<List<Cell>> moves = board.GetAllMoves(color);
            List<Cell> randomMoves;
            while (true)
            {
                randomMoves = moves[random.Next(moves.Count)];
                if (randomMoves.Count > 1) break;
            }

            Cell sourceCell = randomMoves[0];
            Cell targetCell = randomMoves[random.Next(1, randomMoves.Count)];
            sourceCell.MovePiece(targetCell);
        } 
    }
}
