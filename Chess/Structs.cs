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
            this.Cell = cell;
            this.Sprite = sprite;
        }
    }

    public struct SelectedInfo
    {
        public Cell Cell { get; set; }
        public RectangleShape Shape { get; set; }
        public SelectedInfo(Cell cell, RectangleShape shape)
        {
            this.Cell = cell;
            this.Shape = shape;
        }
    }

    public struct MoveInfo
    {
        public Cell Cell { get; set; }
        public CircleShape Circle { get; set; }
        public MoveInfo(Cell cell, CircleShape circle)
        {
            this.Cell = cell;
            this.Circle = circle;
        }
    }
}
