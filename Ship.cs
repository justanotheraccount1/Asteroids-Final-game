using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids_Final_game
{
    internal class Ship
    {
        private Texture2D _texture;
        private Rectangle _rectangle;
        private Vector2 _speed;
        private float _acceleration;
        private float _drag;
        private float _angle;
        private Vector2 _position;
        private Vector2 _direction;
        private Vector2 _momentum;
        private float _dragSeconds;

        public Ship(Texture2D texture)
        {
            _texture = texture;
            _speed = Vector2.Zero;
            _acceleration = 0.0002f;
            _drag = -0.04f;
            _angle = 0;
            _position = new Vector2(390, 240);
            _rectangle = new Rectangle(_position.ToPoint(), new Point(20, 20));
            _direction = Vector2.Zero;
            _momentum = Vector2.Zero;
            _dragSeconds = 0f;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(_rectangle.Center, _rectangle.Size), null, Color.White, _angle + ((float)Math.PI / 2), new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.None, 1f);
        }
        public void Update(Rectangle window, MouseState mouseState, GameTime gameTime)
        {
            
            _direction = mouseState.Position.ToVector2() - _rectangle.Center.ToVector2();
            _angle = (float)Math.Atan2(_direction.Y, _direction.X);
            if (_direction != Vector2.Zero )
            {
                _direction.Normalize();
                _position.X += _momentum.X * _speed.X;
                _position.Y += _momentum.Y * _speed.Y;
                _rectangle.Location = _position.ToPoint();
            }
            
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                _dragSeconds = 0f;
                _momentum = mouseState.Position.ToVector2() - _rectangle.Center.ToVector2();
                if (_speed.X < 0.02)
                    _speed.X += _acceleration;
                if (_speed.Y < 0.02)
                    _speed.Y += _acceleration;
    
            }
            if (mouseState.LeftButton == ButtonState.Released)
            {
                _dragSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_speed.X > 0)
                {
                    _speed.X -= _acceleration;
                }
                if (_speed.Y > 0)
                {
                    _speed.Y -= _acceleration;
                }
                if (_dragSeconds >= 3f)
                {
                    _speed = Vector2.Zero;
                }

            }
            if (_position.X + _rectangle.Width < 0)
            {
                _position.X = window.Width;
            }
            if (_position.X > window.Width)
            {
                _position.X = window.X - _rectangle.Width;
            }
            if (_position.Y + _rectangle.Height < 0)
            {
                _position.Y = window.Height;
            }
            if (_position.Y > window.Height)
            {
                _position.Y = window.Y - _rectangle.Height;
            }
        }
        public Rectangle Rect
        {
            get { return _rectangle; }
        }
    }
}
