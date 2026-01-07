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
        private Vector2 _acceleration;
        private float _angle;
        private Vector2 _position;
        private Vector2 _direction;

        public Ship(Texture2D texture)
        {
            _texture = texture;
            _speed = Vector2.Zero;
            _acceleration = Vector2.Zero;
            _angle = 0;
            _position = new Vector2(100, 100);
            _rectangle = new Rectangle(_position.ToPoint(), new Point(50, 50));
            _direction = Vector2.Zero;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, null, Color.White, _angle, new Vector2(_texture.Width / 2, _texture.Height / 2), SpriteEffects.None, 1f);
        }
        public void Update(Rectangle window, MouseState mouseState)
        {
            
        }
    }
}
