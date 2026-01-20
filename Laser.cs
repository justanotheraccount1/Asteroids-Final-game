using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids_Final_game
{
    internal class Laser
    {
        Rectangle _rectangle;
        Texture2D _texture;
        float _speed;
        Vector2 _location;
        Vector2 _direction;
        int _size;
        
        public Laser(Texture2D texture, Vector2 location, Vector2 target, int size)
        {
            _size = size;
            _texture = texture;
            _location = location;
            _direction = target - location;
            _direction.Normalize();
            _rectangle = new Rectangle(location.ToPoint(), new Point(_size, _size));
            _speed = 8f;

        }
        public void Update()
        {
            _location += _direction * _speed;
            _rectangle.Location = _location.ToPoint();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, Color.White);
        }
        public bool Intersects(Rectangle asteroid)
        {
            return _rectangle.Intersects(asteroid);
        }
    }
}
