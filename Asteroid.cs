using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids_Final_game
{
    internal class Asteroid
    {
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _speed;
        private Random _generator;

        public Asteroid(Texture2D texture, Rectangle location, Vector2 speed)
        {
            _generator = new Random();
            _texture = texture;
            _location = location;
            _speed = new Vector2(_generator.Next(-2, 2), _generator.Next(-2, 2));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }
        public void Update(Rectangle bounds)
        {
            _location.X += (int)_speed.X;
            _location.Y += (int)_speed.Y;
            if (_location.X <= bounds.X)
            {
                _location.X = bounds.X;
                _speed.X *= -1;
            }
            if (_location.Right >= bounds.Right)
            {
                _location.X = bounds.Right - _location.Width;
                _speed.X *= -1;

            }
            if (_location.Y <= bounds.Y)
            {
                _location.Y = bounds.Y;
                _speed.Y *= -1;

            }
            if (_location.Bottom >= bounds.Bottom)
            {
                _location.Y = bounds.Bottom - _location.Height;
                _speed.Y *= -1;

            }
        }
        public Rectangle Rect
        {
            get { return _location; }
        }
    }
}
