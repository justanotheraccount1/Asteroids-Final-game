using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids_Final_game
{
    internal class Particle
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _velocity;
        private float _angle;
        private float _angularVelocity;
        private Color _color;
        private float _size;
        public int _lifespan;

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, float size, Color color)
        {
            _texture = texture;
            _position = position;
            _velocity = velocity;
            _angle = angle;
            _angularVelocity = angularVelocity;
            _color = color;
            _size = size;
            _lifespan = 20;
        }

        public void Update()
        {
            _lifespan--;
            _position += _velocity;
            _angle += _angularVelocity;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
            Vector2 origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

            spriteBatch.Draw(_texture, _position, sourceRectangle, _color, _angle, origin, _size, SpriteEffects.None, 0f);
        }
        public int Lifespan
        {
            get { return _lifespan; }
        }
    }
}
