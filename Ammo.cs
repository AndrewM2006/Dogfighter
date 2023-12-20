using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogfighter
{
    internal class Ammo
    {
        private Texture2D _texture, _circleTexture;
        private Rectangle _circleHitbox, _rectangle;
        private Vector2 _origin, _location;
        private float _angle;
        Random generator = new Random();
        private Color _color;
        private Circle _circle;
        public Ammo(Texture2D texture, Color color, Texture2D circleTexture, GraphicsDeviceManager Graphics)
        {
            _texture = texture;
            _color = color;
            _rectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
            _location = new Vector2(generator.Next(Graphics.PreferredBackBufferWidth - (int)(_texture.Width * 0.4)), generator.Next(Graphics.PreferredBackBufferHeight - (int)(_texture.Height * 0.4)));
            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            _circleTexture = circleTexture;
            _angle = generator.Next(7);
            _circle = new Circle(_location, Convert.ToSingle(_texture.Width * 0.4) / 2);
            _circleHitbox = new Rectangle(
                (int)(_location.X - _circle.Radius),
                (int)(_location.Y - _circle.Radius),
                (int)(_circle.Radius * 2),
                (int)(_circle.Radius * 2)
            );

        }

        public Circle Circle { get { return _circle; } }

        public void Draw(SpriteBatch sprite)
        {
            //sprite.Draw(_circleTexture, _circleHitbox, Color.White);
            sprite.Draw(_texture, _location, _rectangle, _color, _angle, _origin, 0.4f, SpriteEffects.None, 1);
        }
    }
}
