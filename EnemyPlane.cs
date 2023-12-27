using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogfighter
{
    internal class EnemyPlane
    {
        private Texture2D _texture;
        private Texture2D _circleTexture;
        private Rectangle _rectangle, _circleHitbox;
        private Vector2 _location, _origin, _movement;
        private Circle _circle;
        private float _speed, _angle, _firingRate, _seconds, _startTime, _shotSpeed;
        public bool ShotDown = false;
        public List<Shot> shots = new List<Shot> ();
        Random generator = new Random();

        public EnemyPlane(Texture2D texture, float speed, Texture2D circleTexture, float firingRate, GraphicsDeviceManager Graphics, float shotSpeed)
        {
            _texture = texture;
            _speed = speed;
            _rectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
            // Randomly choose a side (0: left, 1: right, 2: top, 3: bottom)
            int spawnSide = generator.Next(4);
            switch (spawnSide)
            {
                case 0: // Left
                    _location = new Vector2(-_texture.Width * 0.1f, generator.Next((int)(Graphics.PreferredBackBufferHeight - _texture.Height * 0.1f)));
                    break;
                case 1: // Right
                    _location = new Vector2(Graphics.PreferredBackBufferWidth, generator.Next((int)(Graphics.PreferredBackBufferHeight - _texture.Height * 0.1f)));
                    break;
                case 2: // Top
                    _location = new Vector2(generator.Next((int)(Graphics.PreferredBackBufferWidth - _texture.Width * 0.1f)), -_texture.Height * 0.1f);
                    break;
                case 3: // Bottom
                    _location = new Vector2(generator.Next((int)(Graphics.PreferredBackBufferWidth - _texture.Width * 0.1f)), Graphics.PreferredBackBufferHeight);
                    break;
                default:
                    break;
            }
            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            _firingRate = firingRate;
            _circleHitbox = new Rectangle(_location.ToPoint(), (new Vector2(Convert.ToSingle(_texture.Width * 0.1), Convert.ToSingle(_texture.Width * 0.1)).ToPoint()));
            _circleTexture = circleTexture;
            _shotSpeed = shotSpeed;
        }

        private void Move(Plane plane)
        {
            _movement = plane.Location() - _location;
            _movement.Normalize();
            _movement *= _speed;
            _location.X += _movement.X;
            _location.Y += _movement.Y;
            UpdateCircle();
            _angle = (float)Math.Atan2(_location.Y - plane.Location().Y, _location.X - plane.Location().X);
        }

        public void UpdateCircle()
        {
            _circleHitbox.Location = _location.ToPoint();
            _circleHitbox.Offset(-_circleHitbox.Width / 2, -_circleHitbox.Height / 2);
            _circle = new Circle(_location, Convert.ToSingle(_texture.Width * 0.1) / 2);
        }

        public void Draw(SpriteBatch sprite)
        {
            //sprite.Draw(_circleTexture, _circleHitbox, Color.White);
            sprite.Draw(_texture, _location, _rectangle, Color.White, _angle, _origin, 0.1f, SpriteEffects.None, 1);
            foreach (Shot shot in shots)
            {
                shot.Draw(sprite);
            }
        }

        public void Update(Plane plane, List<Shot> shot, float totalSeconds, Texture2D shotTexture, GraphicsDeviceManager Graphics)
        {
            _seconds = (float)totalSeconds - _startTime;
            Move(plane);
            ShotCollide(shot);
            if (_seconds >= _firingRate)
            {
                Shoot(shotTexture);
                _startTime = totalSeconds;
            }
            UpdateShot(Graphics);
            plane.ShotCollide(shots);
        }
        public List<Shot> ShotCollide(List<Shot> shot)
        {
            for (int i = 0; i < shot.Count; i++)
            {
                if (_circle.Intersects(shot[i].Circle))
                {
                    shot.RemoveAt(i);
                    ShotDown = true;
                }
            }
            return shot;
        }

        public List<Shot> ShotCollide(List<Shot> shot, int enemy)
        {
            for (int i = 0; i < shot.Count; i++)
            {
                if (_circle.Intersects(shot[i].Circle))
                {
                    shot.RemoveAt(i);
                    ShotDown = true;
                }
            }
            return shot;
        }

        public void SelfCollide(List<EnemyPlane> enemies, int enemyNum)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (_circle.Intersects(enemies[i].Circle) && i!=enemyNum)
                {
                    ShotDown = true;
                }
            }
        }

        public List<Shot> SuperShotCollide(List<Shot> shot)
        {
            for (int i = 0; i < shot.Count; i++)
            {
                if (_circle.Intersects(shot[i].Circle))
                {
                    ShotDown = true;
                }
            }
            return shot;
        }

        private void UpdateShot(GraphicsDeviceManager Graphics)
        {
            for (int i = 0; i < shots.Count; i++)
            {
                shots[i].Update(Graphics);
                if (shots[i].Offscreen)
                {
                    shots.RemoveAt(i);
                }
            }
        }
        
        private void Shoot(Texture2D shotTexture)
        {
            shots.Add(new Shot(shotTexture, _shotSpeed, _angle, _location, _movement/_speed, _circleTexture, Color.Blue));
        }

        public Circle Circle { get { return _circle; } }

    }
}
