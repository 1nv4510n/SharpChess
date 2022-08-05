using System;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using ChessEngine;


namespace Chess
{
    class Game
    {
        private const int WIDTH = 720;
        private const int HEIGHT = 720;
        private const string TITLE = "Chess";
        private RenderWindow window;
        private VideoMode mode = new VideoMode(WIDTH, HEIGHT);
        
        private Sprite background;
        private Board board = new();
        private PiecesManager piecesManager;

        public Game()
        {
            this.window = new RenderWindow(this.mode, TITLE);

            this.window.SetFramerateLimit(30);
            this.window.Closed += (sender, args) => {
                this.window.Close();
            };
            background = SpriteManager.LoadSprite(@"\logo\pure_board.png");

            board.AddPieces();
            piecesManager = new PiecesManager(board);
        }

        public void Run()
        {
            this.piecesManager.UpdatePieces();
            while (this.window.IsOpen)
            {
                // handle
                this.HandleEvents();
                this.Update();
                this.Draw();
            }
        }

        private void HandleEvents()
        {
            this.window.DispatchEvents();
        }
        private void Update() 
        {
            this.piecesManager.MouseHandler(window);
            this.piecesManager.MovePieceHandler(window);
        }

        private void Draw()
        {
            this.window.Clear(Color.Blue);
            this.window.Draw(this.background);
            this.piecesManager.Draw(window);
            this.window.Display();
        }
    }
}