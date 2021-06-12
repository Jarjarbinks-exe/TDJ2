using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoPP2___SpriteDeath_Respawn;

namespace TrabalhoPP2___SpriteDeath_Respawn
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        public static Random Random;

        private float _timer;
        private float _timer2;

        public static int ScreenWidth;
        public static int ScreenHeight;

        private List<Sprites> sprites;
        private bool _hasStarted = false;

        private State _currentState;

        private State _nextState;

        public void ChangeState(State state)
        {
            _nextState = state;
        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Random = new Random();

            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState = new MenuState(this, graphics.GraphicsDevice, Content);
            _font = Content.Load<SpriteFont>("Font");
            Song song = Content.Load<Song>("musica_2");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            Restart();
            // TODO: use this.Content to load your game content here
        }

        private void Restart()
        {
            var playerTexture = Content.Load<Texture2D>("na2");

            sprites = new List<Sprites>()
            {
                new Player(playerTexture)
                {
                    Position = new Vector2((ScreenWidth / 2) - (playerTexture.Width / 2), ScreenHeight - playerTexture.Height),
                    Input = new Input()
                    {
                        Left = Keys.A,
                        Right = Keys.D,
                        Up = Keys.W,
                        Down = Keys.S,
                    },
                    Speed = 10f,
                }
            };

            _hasStarted = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                _currentState = _nextState;

                _nextState = null;
            }

            _currentState.Update(gameTime);

            _currentState.PostUpdate(gameTime);

            if (_nextState != null)
                _hasStarted = true;

            

            if (!_hasStarted)
                return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timer2 += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var sprite in sprites)
            {
                sprite.Update(gameTime, sprites);
            }

            if(_timer > 0.25f)
            {
                _timer = 0;
                sprites.Add(new Bomb(Content.Load<Texture2D>("missil__2")));
            }

            for(int i = 0; i < sprites.Count; i++)
            {
                var sprite = sprites[i];
                if (sprite.isRemoved)
                {
                    sprites.RemoveAt(i);
                    i--;
                }

                if(sprite is Player)
                {
                    var player = sprite as Player;
                    if (player.HasDied)
                    {
                        Restart();
                        _currentState = new MenuState(this, graphics.GraphicsDevice, Content);
                        _currentState.Update(gameTime);
                        _timer2 = 0;
                        
                        if (_nextState != null)
                        {
                            _currentState = _nextState;

                            _nextState = null;
                        }

                        _currentState.PostUpdate(gameTime);
                    }
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            _currentState.Draw(gameTime, _spriteBatch);

            _spriteBatch.Begin();



            foreach(var sprite in sprites)
            {
                sprite.Draw(_spriteBatch);
            }


            int fontY = 10;
            var i = 0;
            foreach (var sprite in sprites)
            {
                if (sprite is Player)
                {
                    _spriteBatch.DrawString(_font, string.Format("Player {0}: {1}", ++i, ((Player)sprite).Score), new Vector2(10, fontY += 20), Color.Black);
                    _spriteBatch.DrawString(_font, string.Format("Time: {0}", (int)_timer2,((Player)sprite).Score) ,new Vector2(10, fontY += 40), Color.Black);
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
