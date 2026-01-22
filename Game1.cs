using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        Win,
        Lose
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState;
        Texture2D shipTexture, smokeTexture, asteroidTexture, laserTexture, boomTexture, titleScreen, winScreen, loseScreen, optionsTexture;
        Ship ship;
        ParticleSystem particleSystem;
        List<Asteroid> asteroids = new List<Asteroid>();
        Random generator = new Random();
        Rectangle window, bounds, virtualRect, optionsRect;
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
        Vector2 scale;
        int score = 0;
        SpriteFont textFont;
        SoundEffect introSound, gameSound, winSound, loseSound, explodeSound;
        SoundEffectInstance introSoundInstance, gameSoundInstance, winSoundInstance, loseSoundInstance, explodeSoundInstance;
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
            bounds = new Rectangle(-100, -100, window.Width + (window.Width/ 10), window.Height + (window.Height / 10));
            virtualWidth = 800;
            virtualHeight = 500;
            virtualRect = new Rectangle(0, 0, 800, 500);
            optionsRect = new Rectangle(10, 10, 30, 30);
            actualWidth = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            actualHeight = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            scaleX = actualWidth / virtualWidth;
            scaleY = actualHeight / virtualHeight;
            scale = new Vector2(scaleX, scaleY);
            //_scale = Math.Min(scaleX, scaleY);


            float viewportWidth = virtualWidth * scaleX;
            float viewportHeight = virtualHeight * scaleY;
            this.Window.Title = $"{actualHeight} {actualWidth}";

            //_screenOffset = new Vector2((actualWidth - viewportWidth) / 2f,(actualHeight - viewportHeight) / 2f);

            scaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
            //scaleMatrix = Matrix.CreateTranslation(new Vector3(_screenOffset, 0)) * Matrix.CreateScale(_scale, _scale, 1f);


            base.Initialize();
            ship = new Ship(shipTexture);
            particleSystem = new ParticleSystem(smokeTexture, new Vector2(400, 240));
            while(!done)
            {
                asteroids.Add(new Asteroid(asteroidTexture, new Rectangle(generator.Next(bounds.X, bounds.Width), generator.Next(bounds.Y, bounds.Height), 20, 20), new Vector2(generator.Next(-2, 3), generator.Next(-2, 3))));
                for (int i = 0; i < asteroids.Count; i++)
                {
                    if (window.Contains(asteroids[i].Rect) || asteroids[i].Speed == Vector2.Zero)
                    {
                        asteroids.RemoveAt(i);
                        i--;
                    }
                }
                if (asteroids.Count > 100)
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
            optionsTexture = Content.Load<Texture2D>("Images/optionsTab");
            titleScreen = Content.Load<Texture2D>("Images/asteroidsTitle");
            textFont = Content.Load<SpriteFont>("Fonts/TextFont");
            introSound = Content.Load<SoundEffect>("Sounds/introMusic");
            gameSound = Content.Load<SoundEffect>("Sounds/mainMusic");
            winSound = Content.Load<SoundEffect>("Sounds/winSound");
            loseSound = Content.Load<SoundEffect>("Sounds/loseSound");
            explodeSound = Content.Load<SoundEffect>("Sounds/explodeSound");
            introSoundInstance = introSound.CreateInstance();
            introSoundInstance.IsLooped = true;
            gameSoundInstance = gameSound.CreateInstance();
            gameSoundInstance.IsLooped = true;
            winSoundInstance = winSound.CreateInstance();
            loseSoundInstance = loseSound.CreateInstance();
            explodeSoundInstance = explodeSound.CreateInstance();

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
                //screenPos -= _screenOffset;

                // Un-scale
                screenPos /= scale;

                return screenPos;
            }
            Vector2 mousePos = GetVirtualMousePosition();

            if (screen == Screen.Title)
            {
                introSoundInstance.Play();
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    screen = Screen.Game;
                }
                if (optionsRect.Contains(mousePos) && mouseState.LeftButton == ButtonState.Pressed)
                {
                    screen = Screen.Options;
                }

            }
            if (screen == Screen.Options)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    screen = Screen.Game;
                }
            }
            if (screen == Screen.Game)
            {
                introSoundInstance.Stop();
                gameSoundInstance.Play();
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

                for (int i = 0; i < lasers.Count; i++)
                {
                    bool removeLaser = false;
                    for (int j = 0; j < asteroids.Count; j++)
                    {
                        if (lasers[i].Intersects(asteroids[j].Rect))
                        {
                            removeLaser = true;
                            boomRects.Add(asteroids[j].Rect);
                            fades.Add(1f);
                            asteroids.RemoveAt(j);
                            j--;
                            score++;
                        }
                    }
                    if (removeLaser)
                    {
                        explodeSound.Play();


                        lasers.RemoveAt(i);
                        i--;
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
                    if (ship.Intersects(asteroids[j].Rect))
                    {
                        screen = Screen.Lose;
                    }
                }
                ship.Update(virtualRect, mouseState, gameTime, mousePos);
                particleSystem.Update();
                for (int i = 0; i < asteroids.Count; i++)
                {
                    if (asteroids.Count == 0 || asteroids.Count < 10 && !virtualRect.Contains(asteroids[i].Rect) || score >= 85)
                    {
                        screen = Screen.Win;
                    }
                }



            }
            if (screen == Screen.Win)
            {
                gameSoundInstance.Stop();
                winSoundInstance.Play();
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    screen = Screen.Title;
                }
            }
            if (screen == Screen.Lose)
            {
                gameSoundInstance.Stop();
                loseSoundInstance.Play();
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    screen = Screen.Title;
                }
            }

            // TODO: Add your update logic here
            prevMouseState = mouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Vector2 GetVirtualMousePosition()
            {
                var mouse = Mouse.GetState();

                Vector2 screenPos = new Vector2(mouse.X, mouse.Y);

                // Remove letterbox offset
                //screenPos -= _screenOffset;

                // Un-scale
                screenPos /= scale;

                return screenPos;
            }
            Vector2 mousePos = GetVirtualMousePosition();

            _spriteBatch.Begin(transformMatrix: scaleMatrix);

            if (screen == Screen.Title)
            {

                _spriteBatch.Draw(titleScreen, virtualRect, Color.White);
                
                if (optionsRect.Contains(mousePos))
                {
                    _spriteBatch.Draw(optionsTexture, new Rectangle(8, 8, 34, 34), Color.Turquoise);
                }
                _spriteBatch.Draw(optionsTexture, new Rectangle(10, 10, 30, 30), Color.White);
            }
            if (screen == Screen.Options)
            {
                _spriteBatch.DrawString(textFont, "Left Click", new Vector2(100, 200), Color.White);
                _spriteBatch.DrawString(textFont, "Boosters", new Vector2(100, 250), Color.White);
                _spriteBatch.DrawString(textFont, "Right Click", new Vector2(100, 400), Color.White);
                _spriteBatch.DrawString(textFont, "Lasers", new Vector2(100, 450), Color.White);
                _spriteBatch.DrawString(textFont, "ENTER to start", new Vector2(100, 50), Color.White);
            }
            if (screen == Screen.Game)
            {
                particleSystem.Draw(_spriteBatch);
                for (int i = 0; i < lasers.Count; i++)
                {
                    lasers[i].Draw(_spriteBatch);
                }
                for (int i = 0; i < boomRects.Count; i++)
                {
                    _spriteBatch.Draw(boomTexture, boomRects[i], Color.White * fades[i]);
                }
                ship.Draw(_spriteBatch);
                for (int i = 0; i < asteroids.Count; i++)
                {
                    asteroids[i].Draw(_spriteBatch);
                }
            }
            if(screen == Screen.Win)
            {
                _spriteBatch.DrawString(textFont, "YOU WIN!", new Vector2(308, 248), Color.DarkGoldenrod);
                _spriteBatch.DrawString(textFont, "YOU WIN!", new Vector2(312, 252), Color.Yellow);
                _spriteBatch.DrawString(textFont, "YOU WIN!", new Vector2(310, 250), Color.Gold);
                

            }
            if (screen == Screen.Lose)
            {
                _spriteBatch.DrawString(textFont, "you lose...", new Vector2(310, 250), Color.White);

                _spriteBatch.DrawString(textFont, "Try Again", new Vector2(310, 450), Color.White);

            }
            _spriteBatch.End();


            //Vector2 screenMousePos = new Vector2(mouseState.X, mouseState.Y);
            //Matrix invertMatrix = Matrix.Invert(scaleMatrix);
            //Vector2 virtualMousePos = Vector2.Transform(screenMousePos, invertMatrix);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
