using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids_Final_game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        MouseState mouseState;
        Texture2D shipTexture, smokeTexture, asteroidTexture;
        Ship ship;
        ParticleSystem particleSystem;

        Rectangle window;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            window = new Rectangle(0, 0, 800, 500);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            base.Initialize();
            ship = new Ship(shipTexture);
            particleSystem = new ParticleSystem(smokeTexture, new Vector2(400, 240));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            shipTexture = Content.Load<Texture2D>("Images/ship");
            smokeTexture = Content.Load<Texture2D>("Images/smokePuff");
            asteroidTexture = Content.Load<Texture2D>("Images/spaceRock");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                particleSystem.EmitterLocation = new Vector2(ship.Rect.X + (ship.Rect.Width / 2), ship.Rect.Y + (ship.Rect.Height / 2));
                particleSystem.Enabled = true;
            }
            else
                particleSystem.Enabled = false;

                ship.Update(window, mouseState, gameTime);
            particleSystem.Update();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            particleSystem.Draw(_spriteBatch);
            ship.Draw(_spriteBatch);
            
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
