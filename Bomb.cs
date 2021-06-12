using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TrabalhoPP2___SpriteDeath_Respawn
{
    public class Bomb : Sprites
    {
        public Bomb(Texture2D texture)
            : base(texture)
        {
            Position = new Vector2(Game1.Random.Next(0, Game1.ScreenWidth - _texture.Width), -_texture.Height);
            Speed = Game1.Random.Next(3, 10);
        }

        public override void Update(GameTime gameTime, List<Sprites> sprite)
        {
            Position.Y += Speed;
            if (Rectangle.Bottom >= Game1.ScreenHeight)
            {
                isRemoved = true;
            }

        }
    }
}
