using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;

namespace ChessEngine
{
    internal class Board
    {
        internal Cell[,] cells = new Cell[8, 8];
        internal Board()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < cells.Length; j++)
                {
                    if ((i + j) % 2 != 0)
                    {
                        cells[j, i] = new Cell(this, j, i, Colors.BLACK, null);
                    } 
                    else
                    {
                        cells[j, i] = new Cell(this, j, i, Colors.WHITE, null);
                    }
                }
            }
        }
        internal Cell GetCell (int x, int y)
        {
            return cells[y, x];
        }

        internal Cell GetCellFromPgn (string cell)
        {
            string horizontal = "abcdefgh";
            string vertical = "87654321";
            return cells[vertical.IndexOf(cell[1]), horizontal.IndexOf(cell[0])];
        }
        
    }
}
