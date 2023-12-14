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
        private Rectangle _rectangle;
        private Vector2 _location, _origin;

        MouseState _mouseState;
        private float _speed, _deltaX, _deltaY, _angle;

        public Plane(Texture2D texture, float speed)
        {
            _texture = texture;
            _speed = speed;
            _rectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
            _location = new Vector2(100, 100);
            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        public Vector2 Location { get { return _location; } }

        public Vector2 Update(GraphicsDeviceManager Graphics)
        {
            Move(Graphics);
            return _location;
        }

        private void Move(GraphicsDeviceManager Graphics)
        {
            _mouseState = Mouse.GetState();
            _angle = (float)Math.Atan2(_location.Y - _mouseState.Y, _location.X - _mouseState.X);
            _deltaX = _speed * (float)Math.Cos(_angle);
            _deltaY = _speed * (float)Math.Sin(_angle);
            _location.X -= _deltaX;
            if (_location.X + _rectangle.Width*0.1 > Graphics.PreferredBackBufferWidth || _location.X-_rectangle.Width*0.1 < 0)
            {
                _location.X += _deltaX;
            }
            _location.Y -= _deltaY;
            if (_location.Y + _rectangle.Height*0.1 > Graphics.PreferredBackBufferHeight || _location.Y - _rectangle.Height*0.1 < 0)
            {
                _location.Y += _deltaY;
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(_texture, _location, _rectangle, Color.White, _angle, _origin, 0.1f, SpriteEffects.None, 1);
        }
    }
}
