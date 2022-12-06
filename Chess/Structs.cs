using SFML.Graphics;
using ChessEngine;

namespace Chess
{
    public struct PieceInfo
    {
        public Cell Cell { get; set; }
        public Sprite Sprite { get; set; }
        public PieceInfo(Cell cell, Sprite sprite)
        {
            Cell = cell;
            Sprite = sprite;
        }
    }

    public struct SelectedInfo
    {
        public Cell Cell { get; set; }
        public RectangleShape Shape { get; set; }
        public SelectedInfo(Cell cell, RectangleShape shape)
        {
            Cell = cell;
            Shape = shape;
        }
    }

    public struct MoveInfo
    {
        public Cell Cell { get; set; }
        public CircleShape Circle { get; set; }
        public MoveInfo(Cell cell, CircleShape circle)
        {
            Cell = cell;
            Circle = circle;
        }
    }
}
