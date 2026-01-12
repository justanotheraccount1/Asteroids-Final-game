using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids_Final_game
{
    public class ParticleSystem
    {
        private Random _generator;
        public Vector2 _emitterLocation { get; set; }
        private List<Particle> particles;
        private Texture2D _texture;
        private bool _enabled;


        public ParticleSystem(Texture2D texture, Vector2 location)
        {
            _emitterLocation = location;
            _texture = texture;
            this.particles = new List<Particle>();
            _generator = new Random();
            _enabled = false;
        }
        private Particle GenerateNewParticle()
        {
            //if (_generator.Next(3) == 0)
            //{
            //    Color color = Color.White;
            //}
            //else if (_generator.Next(3) == 1)
            //{
            //    Color color = Color.Yellow;
            //}
            //else if (_generator.Next(3) == 1)
            //{
            //    Color color = Color.Orange;
            //}
            //else
            //{
            //    Color color = Color.Red;

            //}
            Vector2 _position = _emitterLocation;
            Vector2 _velocity = new Vector2(
                    1f * (float)(_generator.NextDouble() * 2 - 1),
                    1f * (float)(_generator.NextDouble() * 2 - 1));
            float _angle = 0;
            float _angularVelocity = 0.1f * (float)(_generator.NextDouble() * 2 - 1);
            float size = (float)_generator.NextDouble();

            return new Particle(_texture, _position, _velocity, _angle, _angularVelocity, size, color);
        }
        public void Update()
        {
            int total = 10;

            if (_enabled) 
                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewParticle());
                }

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update();
                if (particles[i].Lifespan <= 0)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Draw(spriteBatch);
            }
        }
        public Vector2 EmitterLocation
        {
            get { return _emitterLocation; }
            set { _emitterLocation = value; }
        }
        public bool Enabled
        {
            get { return _enabled; } 
            set { _enabled = value; }
        }

    }
}
