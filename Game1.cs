using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Asteroids_Final_game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        MouseState mouseState, prevMouseState;
        Texture2D shipTexture, smokeTexture, asteroidTexture;
        Ship ship;
        ParticleSystem particleSystem;
        List<Asteroid> asteroids = new List<Asteroid>();
        Random generator = new Random();
        Rectangle window, bounds;
        bool done = false;
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
            bounds = new Rectangle(-100, -100, 1000, 700);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            base.Initialize();
            ship = new Ship(shipTexture);
            particleSystem = new ParticleSystem(smokeTexture, new Vector2(400, 240));
            while(!done)
            {
                asteroids.Add(new Asteroid(asteroidTexture, new Rectangle(generator.Next(-100, 900), generator.Next(-100, 600), 20, 20), new Vector2(generator.Next(-2, 3), generator.Next(-2, 3))));
                for (int i = 0; i < asteroids.Count; i++)
                {
                    if (window.Contains(asteroids[i].Rect))
                    {
                        asteroids.RemoveAt(i);
                        i--;
                    }
                }
                if (asteroids.Count > 50)
                {
                    done = true;
                }
            }
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
            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton != ButtonState.Pressed)
            {

            }
            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].Update(bounds);
            }
            ship.Update(window, mouseState, gameTime);
            particleSystem.Update();
            // TODO: Add your update logic here
            prevMouseState = mouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            particleSystem.Draw(_spriteBatch);
            ship.Draw(_spriteBatch);
            for(int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].Draw(_spriteBatch);
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
