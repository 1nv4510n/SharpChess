using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ChessEngine.Enum;

namespace ChessEngine
{
    public class PlayerStruct
    {
        public struct Player
        {
            public readonly Colors Color { get; init; }
            public readonly PlayerType Type { get; init; }
            public ChessStatus? Status { get; set; }
            public ChessStatus? GameResult { get; set; }

            public Player(Colors color, PlayerType playerType)
            {
                Color = color;
                Type = playerType;
                Status = null;
                GameResult = null;
            }
            
            public override bool Equals(Object obj)
            {
                return obj is Player p && this == p;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Color, Status, GameResult);
            }

            public static bool operator ==(Player p1, Player p2)
            {
                return p1.Color == p2.Color;
            }

            public static bool operator !=(Player p1, Player p2)
            {
                return !(p1 == p2);
            }
        }
    }
}
