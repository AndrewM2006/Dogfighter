using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Dogfighter
{
    internal class Plane
    {
        private Texture2D _texture;
        private Texture2D _circleTexture;
        private Rectangle _rectangle, _circleHitbox;
        private Vector2 _location, _origin, _destination, _movement;
        private Circle _circle;
        MouseState _mouseState;
        private float _speed, _angle;
        public int _ammo, _superammo;
        public bool _dead;

        public Plane(Texture2D texture, float speed, Texture2D circleTexture, int startAmmo, int startSuper)
        {
            _texture = texture;
            _speed = speed;
            _rectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
            _location = new Vector2(100, 100);
            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            _circleHitbox = new Rectangle(_location.ToPoint(), (new Vector2(Convert.ToSingle(_texture.Width * 0.1), Convert.ToSingle(_texture.Width * 0.1)).ToPoint()));
            _circleTexture = circleTexture;
            _ammo = startAmmo;
            _superammo = startSuper;
            _dead = false;
        }

        public void Update(GraphicsDeviceManager Graphics, List<Ammo> ammo, List<EnemyPlane> enemies)
        {
            Move(Graphics);
            AmmoCollide(ammo);
            EnemyCollide(enemies);
        }

        private void Move(GraphicsDeviceManager Graphics)
        {
            _mouseState = Mouse.GetState();
            if ((_location.Y > _mouseState.Y + _speed/2 || _location.Y < _mouseState.Y - _speed / 2) && (_location.X > _mouseState.X + _speed / 2 || _location.X < _mouseState.X - _speed / 2))
            {
                _destination = new Vector2(_mouseState.X, _mouseState.Y);
                _movement = _destination - _location;
                _movement.Normalize();
                _movement *= _speed;
                _location.X += _movement.X;
                UpdateCircle();
                if (_circle.BoundaryX(Graphics))
                {
                    _location.X -= _movement.X;
                    UpdateCircle();
                }
                _location.Y += _movement.Y;
                UpdateCircle();
                if (_circle.BoundaryY(Graphics))
                {
                    _location.Y-=_movement.Y;
                    UpdateCircle();
                }
                _angle = (float)Math.Atan2(_location.Y - _mouseState.Y, _location.X - _mouseState.X);
            }
        }

        public List<Ammo> AmmoCollide(List<Ammo> ammo)
        {
            for (int i = 0; i < ammo.Count; i++)
            {
                if (_circle.Intersects(ammo[i].Circle))
                {
                    if (ammo[i]._color == Color.Red)
                    {
                        _ammo++;
                    }
                    else
                    {
                        _superammo++;
                    }
                    ammo.RemoveAt(i);
                }
            }
            return ammo;
        }

        public void ShotCollide(List<Shot> shots)
        {
            foreach (Shot shot in shots)
            {
                if (_circle.Intersects(shot.Circle))
                {
                    _dead = true;
                }
            }
        }

        public void EnemyCollide(List<EnemyPlane> enemies)
        {
            foreach (EnemyPlane enemy in enemies)
            {
                if (_circle.Intersects(enemy.Circle))
                {
                    _dead = true;
                }
            }
        }

        public float Angle()
        {
            return _angle;
        }

        public Vector2 Movement()
        {
            return _movement/=_speed;
        }

        public Vector2 Location()
        {
            return _location;
        }
        public void Draw(SpriteBatch sprite)
        {
            //sprite.Draw(_circleTexture, _circleHitbox, Color.White);
            sprite.Draw(_texture, _location, _rectangle, Color.White, _angle, _origin, 0.1f, SpriteEffects.None, 1);
        }

        public void UpdateCircle()
        {
            _circleHitbox.Location = _location.ToPoint();
            _circleHitbox.Offset(-_circleHitbox.Width / 2, -_circleHitbox.Height / 2);
            _circle = new Circle(_location, Convert.ToSingle(_texture.Width * 0.1) / 2);
        }
    }
}
