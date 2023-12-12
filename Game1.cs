using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Dogfighter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D planeTexture;
        MouseState mouseState;
        Vector2 location, origin;
        Rectangle sourceRectangle;
        float angle = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            location = new Vector2(100, 100);
            sourceRectangle = new Rectangle(0, 0, planeTexture.Width, planeTexture.Height);
            origin = new Vector2(planeTexture.Width, planeTexture.Height / 2);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            planeTexture = Content.Load<Texture2D>("airplane");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            mouseState = Mouse.GetState();
            angle = (float)Math.Atan2(location.Y - mouseState.Y, location.X - mouseState.X);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(planeTexture, location, sourceRectangle, Color.White, angle, origin, 0.1f, SpriteEffects.None, 1);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}