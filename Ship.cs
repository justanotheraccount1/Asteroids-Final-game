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

        public Ship(Texture2D texture)
        {
            _texture = texture;
            _speed = Vector2.Zero;
            _acceleration = 0.04f;
            _drag = -0.01f;
            _angle = 0;
            _position = new Vector2(375, 225);
            _rectangle = new Rectangle(_position.ToPoint(), new Point(50, 50));
            _direction = Vector2.Zero;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(_rectangle.Center, _rectangle.Size), null, Color.White, _angle + ((float)Math.PI / 2), new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.None, 1f);
        }
        public void Update(Rectangle window, MouseState mouseState)
        {
            _direction = mouseState.Position.ToVector2() - _rectangle.Center.ToVector2();
            _angle = (float)Math.Atan2(_direction.Y, _direction.X);
            if (_direction != Vector2.Zero )
            {
                _direction.Normalize();
                _position.X += _direction.X * _speed.X;
                _position.Y += _direction.Y * _speed.Y;
                _rectangle.Location = _position.ToPoint();
            }
            
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                _speed.X += _acceleration;
                _speed.Y += _acceleration;
            }
            if(mouseState.LeftButton == ButtonState.Released)
            {
                if (_speed.X > 0)
                {
                    _speed.X += _drag;
                }
                if (_speed.Y > 0)
                {
                    _speed.Y += _drag;
                }
                
                
            }

        }
    }
}
