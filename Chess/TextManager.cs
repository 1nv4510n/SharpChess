using SFML.System;
using SFML.Window;
using SFML.Graphics;

using ChessEngine;
using static ChessEngine.Enum;

namespace Chess
{
    class TextManager
    {
        public Text text;
        private ChessGame game;
        
        public TextManager(ChessGame game, string fontPath)
        {
            text = new Text("", new Font(fontPath), 60);
            this.game = game;

            text.Position = new Vector2f(720 / 3 - 50, 720 / 3 - 50);
            text.Style = Text.Styles.Bold;
            text.FillColor = Color.White;
            text.OutlineColor = Color.Black;
            text.OutlineThickness = 5;
        }


        public void Update()
        {
            if (game.isGameOver)
            {
                if (game.endGameStatus == ChessStatus.CHECKMATE)
                {
                    switch (game.currentTurn.Status)
                    {
                        case ChessStatus.LOSE:
                            text.DisplayedString = "GAME OVER\nYOU WIN\nCHECKMATE";
                            break;
                        case ChessStatus.WIN:
                            text.DisplayedString = "GAME OVER\nYOU LOSE\nCHECKMATE";
                            break;
                    }
                } else if (game.endGameStatus == ChessStatus.STALEMATE)
                {
                    text.DisplayedString = "GAME OVER\nDRAW\nSTALEMATE";
                }
            }
        }

        public void Draw(RenderTarget window)
        {
            window.Draw(text);
        }
    }
}
