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
    internal class Achievements
    {
        private Texture2D _rectangleTexture, _coinTexture;
        private Rectangle _location;
        private Color _color;
        public bool _achieved = false;
        public bool _collected = false;
        private string _name;
        private string _description;
        private SpriteFont _font;
        private string _text;
        private float _worth;

        public Achievements(Texture2D Rectangle, Texture2D Coin, Rectangle Location, Color Color, string Name, string Description, SpriteFont font, float Worth)
        {
            _rectangleTexture = Rectangle;
            _coinTexture = Coin;
            _location = Location;
            _color = Color;
            _name = Name;
            _description = Description;
            _font = font;
            _text = Name;
            _worth = Worth;
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(_rectangleTexture, _location, _color);
            if (!_achieved)
            {
                sprite.Draw(_rectangleTexture, new Rectangle(_location.X + 324, _location.Y + 2, 56, 56), Color.DarkGray);
                sprite.Draw(_coinTexture, new Rectangle(_location.X + 327, _location.Y + 5, 50, 50), Color.DarkGray);
                sprite.DrawString(_font, _text, new Vector2(_location.X+12, _location.Y+12), Color.White);
            }
            else if (_achieved && !_collected)
            {
                sprite.Draw(_rectangleTexture, new Rectangle(_location.X + 324, _location.Y + 2, 56, 56), Color.Gold);
                sprite.Draw(_coinTexture, new Rectangle(_location.X + 327, _location.Y + 5, 50, 50), Color.White);
                sprite.DrawString(_font, _text, new Vector2(_location.X + 12, _location.Y + 12), Color.White);
            }
            else
            {
                sprite.DrawString(_font, _text, new Vector2(_location.X + 12, _location.Y + 12), Color.Gold);
            }
        }

        public float Update(MouseState mouse, float coins)
        {
            if (_location.Contains(mouse.X, mouse.Y) && mouse.LeftButton == ButtonState.Released)
            {
                _text = _description;
            }
            else if (_location.Contains(mouse.X, mouse.Y) && mouse.LeftButton == ButtonState.Pressed && _achieved && !_collected)
            {
                _collected = true;
                coins += _worth;
            }
            else
            {
                _text = _name;
            }
            return coins;
        }
    }
}
