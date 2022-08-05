using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using SFML.Graphics;
using SFML.Window;
using SFML.System;
using ChessEngine;

using static ChessEngine.Enum;


namespace Chess
{
    class PiecesManager
    {
        private List<PieceInfo> pieces = new List<PieceInfo>();
        private SelectedInfo? selectedPiece = null;
        private List<MoveInfo> selectedMoves = new List<MoveInfo>();
        private Board board;

        public PiecesManager(Board board)
        {
            this.board = board;
        }

        public void UpdatePieces()
        {
            pieces.Clear();
            foreach (var row in board.cells)
            {
                foreach (var targetCell in row)
                {
                    if (targetCell.piece is not null)
                    {
                        PieceInfo pieceInfo = new(targetCell, SpriteManager.LoadSprite(targetCell.piece.logo));
                        float x = targetCell.x * (int)BoardParam.CELL_SIZE + (int)BoardParam.OFFSET_X;
                        float y = targetCell.y * (int)BoardParam.CELL_SIZE + (int)BoardParam.OFFSET_Y;
                        pieceInfo.Sprite.Position = new Vector2f(x, y);
                        pieces.Add(pieceInfo);
                    }
                }
            }
        }

        public void MouseHandler(RenderWindow window)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                foreach (var pieceInfo in pieces)
                {
                    var mousePos = Mouse.GetPosition(window);
                    if (pieceInfo.Sprite.GetGlobalBounds().Contains(mousePos.X, mousePos.Y))
                    {
                        if (selectedPiece is null)
                        {
                            RectangleShape border = new(new Vector2f((int)BoardParam.CELL_SIZE, (int)BoardParam.CELL_SIZE));

                            border.Position = pieceInfo.Sprite.Position;
                            border.FillColor = Color.Transparent;
                            border.OutlineThickness = 4;
                            border.OutlineColor = Color.Red;
                            selectedPiece = new SelectedInfo(pieceInfo.Cell, border);
                            selectedMoves.Clear();

                            foreach (var move in pieceInfo.Cell.board.HighlightMoves(pieceInfo.Cell))
                            {
                                Cell cell = board.GetCellFromPgn(move);
                                float posX = cell.x * ((int)BoardParam.CELL_SIZE) + (int)BoardParam.CELL_SIZE / 2 + (int)BoardParam.OFFSET_X / 2 + 8;
                                float posY = cell.y * ((int)BoardParam.CELL_SIZE) + (int)BoardParam.CELL_SIZE / 2 + (int)BoardParam.OFFSET_Y / 2 + 5;
                                CircleShape circle = new CircleShape();
                                circle.Position = new Vector2f(posX, posY);
                                circle.FillColor = Color.Green;
                                circle.Radius = 10;
                                selectedMoves.Add(new MoveInfo(cell, circle));
                            }
                        }
                    }
                }
            }
        }

        public void MovePieceHandler(RenderWindow window)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Right) && selectedPiece is not null)
            {
                selectedPiece = null;
                selectedMoves.Clear();
            }

            while (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (selectedPiece is not null)
                {
                    foreach (var moveInfo in selectedMoves)
                    {
                        var mousePos = Mouse.GetPosition(window);
                        if (moveInfo.Circle.GetGlobalBounds().Contains(mousePos.X, mousePos.Y))
                        {
                            selectedPiece?.Cell.MovePiece(moveInfo.Cell);
                            selectedPiece = null;
                            selectedMoves.Clear();
                            UpdatePieces();
                            Thread.Sleep(100);
                            return;
                        }
                    }
                }
            }
        }
        
        public void Draw(RenderTarget window)
        {
            foreach (var pieceInfo in pieces)
            {
                window.Draw(pieceInfo.Sprite);
            }
            if (selectedPiece is not null)
            {
                window.Draw(selectedPiece?.Shape);
                foreach (var move in selectedMoves)
                {
                    window.Draw(move.Circle);
                }
            }
        }
    }
}
