using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TrabalhoPP2___SpriteDeath_Respawn
{
    public class Player : Sprites
    {
        public bool HasDied = false;
        public int Score;

        public Player(Texture2D texture)
            : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<Sprites> sprite)
        {
            Move();

            foreach (var Sprite in sprite)
            {
                if (Sprite == this)
                    continue;

                if (Sprite.Rectangle.Intersects(this.Rectangle))
                {
                    this.HasDied = true;
                }
            }
            Position += Velocity;

            // Keep the sprite on the screen
            Position.X = MathHelper.Clamp(Position.X, 0, Game1.ScreenWidth - Rectangle.Width);

            // Reset the velociy for when the user isnt holding down a key
            Velocity = Vector2.Zero;

        }

        private void Move()
        {
            if (Input == null)
                throw new Exception("Please assign a value to 'Input'");

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                Speed = 15f;
            else
                Speed = 10f;
            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Velocity.Y = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
                Velocity.Y = Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.X = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.X = Speed;
        }

    }
}
