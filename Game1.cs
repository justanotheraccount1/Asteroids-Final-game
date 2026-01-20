using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Asteroids_Final_game
{
    enum Screen
    {
        Title,
        Game,
        Options,
        Lose
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState;
        Texture2D shipTexture, smokeTexture, asteroidTexture, laserTexture, boomTexture;
        Ship ship;
        ParticleSystem particleSystem;
        List<Asteroid> asteroids = new List<Asteroid>();
        Random generator = new Random();
        Rectangle window, bounds, virtualRect;
        bool done = false;
        List<Laser> lasers = new List<Laser>();
        List<Rectangle> boomRects = new List<Rectangle>();
        List<float> fades = new List<float>();
        int virtualWidth, virtualHeight;
        float actualWidth, actualHeight;
        float scaleX, scaleY;
        Matrix scaleMatrix;
        float _scale;
        Vector2 _screenOffset;
        Screen screen;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            window = new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();
            bounds = new Rectangle(-100, -100, 1000, 700);
            virtualWidth = 800;
            virtualHeight = 500;
            virtualRect = new Rectangle(0, 0, 800, 500);
            actualWidth = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            actualHeight = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            scaleX = actualWidth / virtualWidth;
            scaleY = actualHeight / virtualHeight;
            _scale = Math.Min(scaleX, scaleY);


            float viewportWidth = virtualWidth * _scale;
            float viewportHeight = virtualHeight * _scale;
            this.Window.Title = $"{actualHeight} {actualWidth}";

            _screenOffset = new Vector2((actualWidth - viewportWidth) / 2f,(actualHeight - viewportHeight) / 2f);
            scaleMatrix = Matrix.CreateTranslation(new Vector3(_screenOffset, 0)) * Matrix.CreateScale(_scale, _scale, 1f);


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
            laserTexture = Content.Load<Texture2D>("Images/energyBall");
            boomTexture = Content.Load<Texture2D>("Images/boom");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            Vector2 GetVirtualMousePosition()
            {
                var mouse = Mouse.GetState();

                Vector2 screenPos = new Vector2(mouse.X, mouse.Y);

                // Remove letterbox offset
                screenPos -= _screenOffset;

                // Un-scale
                screenPos /= _scale;

                return screenPos;
            }
            Vector2 mousePos = GetVirtualMousePosition();

            if (screen == Screen.Title)
            {
                if
            }
            if (screen == Screen.Game)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    particleSystem.EmitterLocation = new Vector2(ship.Rect.X + (ship.Rect.Width / 2), ship.Rect.Y + (ship.Rect.Height / 2));
                    particleSystem.Enabled = true;
                }
                else
                    particleSystem.Enabled = false;
                if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton != ButtonState.Pressed)
                {
                    lasers.Add(new Laser(laserTexture, ship.Rect.Center.ToVector2(), mousePos, 10));
                }

                for (int i = 1; i < lasers.Count; i++)
                {
                    for (int j = 0; j < asteroids.Count; j++)
                    {
                        if (lasers[i].Intersects(asteroids[j].Rect))
                        {
                            lasers.RemoveAt(i);
                            boomRects.Add(asteroids[j].Rect);
                            fades.Add(1f);
                            asteroids.RemoveAt(j);
                            i--;
                            j--;
                        }
                    }
                }
                for (int i = 0; i < fades.Count; i++)
                {
                    fades[i] -= 0.05f;
                    if (fades[i] <= 0)
                    {
                        fades.RemoveAt(i);
                        boomRects.RemoveAt(i);
                        i--;
                    }
                }
                for (int i = 0; i < lasers.Count; i++)
                {
                    lasers[i].Update();
                }
                for (int j = 0; j < asteroids.Count; j++)
                {
                    asteroids[j].Update(bounds);
                }
                ship.Update(virtualRect, mouseState, gameTime, mousePos);
                particleSystem.Update();
            }

            
            // TODO: Add your update logic here
            prevMouseState = mouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            _spriteBatch.Begin(transformMatrix: scaleMatrix);


            particleSystem.Draw(_spriteBatch);
            for(int i = 0; i < lasers.Count; i++)
            {
                lasers[i].Draw(_spriteBatch);
            }
            for (int i = 0; i < boomRects.Count; i++)
            {
                _spriteBatch.Draw(boomTexture, boomRects[i], Color.White * fades[i]);
            }
            ship.Draw(_spriteBatch);
            for(int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].Draw(_spriteBatch);
            }
            
            _spriteBatch.End();


            Vector2 screenMousePos = new Vector2(mouseState.X, mouseState.Y);
            Matrix invertMatrix = Matrix.Invert(scaleMatrix);
            Vector2 virtualMousePos = Vector2.Transform(screenMousePos, invertMatrix);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
