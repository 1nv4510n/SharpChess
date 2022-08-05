using System;
using SFML.Graphics;
using SFML.Window;

namespace Chess
{
    static class SpriteManager
    {
        private static string ASSETS_PATH = @"Pieces";

        public static Sprite LoadSprite(string filePath)
        {
            Sprite sprite = new();
            Texture texture = new Texture(ASSETS_PATH + filePath);
            sprite.Texture = texture;
            return sprite;
        }
    }
}
