using ChessEngine;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

using static ChessEngine.ChessStruct;
using static ChessEngine.Enum;


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
        private ChessGame game;
        private PiecesManager piecesManager;
        private TextManager textManager;

        public Game()
        {
            window = new RenderWindow(mode, TITLE);

            window.SetFramerateLimit(30);
            window.Closed += (sender, args) =>
            {
                window.Close();
            };
            background = SpriteManager.LoadSprite(@"\logo\pure_board.png");

            Player human = new(Colors.WHITE, PlayerType.HUMAN);
            Player bot = new(Colors.BLACK, PlayerType.COMPUTER);

            game = new ChessGame(human, bot);
            piecesManager = new PiecesManager(game);
            textManager = new TextManager(game, "fonts/NotoSans.ttf");

        }

        private void HandleEvents()
        {
            window.DispatchEvents();
        }
        private void Update()
        {
            if (!game.isGameOver)
            {
                piecesManager.MouseHandler(window);
                piecesManager.MovePieceHandler(window);
                var e = game.ComputerMove();
                if (e is not null)
                {
                    piecesManager.UpdatePieces();
                    SoundManager.PlaySound(e);
                }
            }
            textManager.Update();
        }

        private void Draw()
        {
            window.Clear(Color.Blue);
            window.Draw(background);
            piecesManager.Draw(window);
            textManager.Draw(window);
            window.Display();
        }
        public void Run()
        {
            piecesManager.UpdatePieces();
            while (window.IsOpen)
            {
                HandleEvents();
                Update();
                Draw();
            }
        }
    }
}