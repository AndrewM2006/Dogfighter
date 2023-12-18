using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogfighter
{
    internal class Shot
    {
        private Texture2D _texture, _circleTexture;
        private float _speed, _angle;
        private Vector2 _location, _origin, _movement;
        private Circle _circle;
        private Rectangle _circleHitbox, _rectangle;
        public bool Offscreen = false;

        public Shot(Texture2D texture, float speed, float angle, Vector2 location, Vector2 movement, Texture2D circleTexture)
        {
            _texture = texture;
            _speed = speed;
            _angle = angle;
            _location = location;
            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            _movement = movement * speed;
            _circleHitbox = new Rectangle(_location.ToPoint(), (new Vector2(Convert.ToSingle(_texture.Width * 0.03), Convert.ToSingle(_texture.Width * 0.03)).ToPoint()));
            _circleTexture = circleTexture;
            _rectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
        }

        private void Move(GraphicsDeviceManager Graphics)
        {
            _location.X += _movement.X;
            _location.Y += _movement.Y;
            UpdateCircle();
            if (_circle.IsOffscreen(Graphics))
            {
                Offscreen = true;
            }
            
        }

        public void Update(GraphicsDeviceManager Graphics)
        {
            Move(Graphics);
        }

        public void UpdateCircle()
        {
            _circleHitbox.Location = _location.ToPoint();
            _circleHitbox.Offset(-_circleHitbox.Width / 2, -_circleHitbox.Height / 2);
            _circle = new Circle(_location, Convert.ToSingle(_texture.Width * 0.03) / 2);
        }

        public void Draw(SpriteBatch sprite)
        {
            //sprite.Draw(_circleTexture, _circleHitbox, Color.White);
            sprite.Draw(_texture, _location, _rectangle, Color.White, _angle, _origin, 0.03f, SpriteEffects.None, 1);
        }
    }
}
