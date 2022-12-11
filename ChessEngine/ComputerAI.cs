using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;
using static ChessEngine.ChessStruct;

namespace ChessEngine
{
    public class ComputerAI
    {
        private readonly Board board;
        private readonly Random random = new();
        public ComputerAI(Board board)
        {
            this.board = board;
        }
        
        public Move GetMove(Colors color) {
            List<List<Cell>> moves = board.GetAllMoves(color);
            List<Cell> randomMoves;
            while (true)
            {
                randomMoves = moves[random.Next(moves.Count)];
                Console.WriteLine($"{moves.Count} {color}");
                if (randomMoves.Count > 1 || moves.Count == 0) break;
            }

            Cell sourceCell = randomMoves[0];
            Cell targetCell = randomMoves[random.Next(1, randomMoves.Count)];
            return new Move(color, sourceCell, targetCell);
        } 
    }
}
