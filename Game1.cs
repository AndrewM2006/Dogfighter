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
        private Texture2D circkleTexture;
        Rectangle circleHitbox;

        Plane plane;
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
            plane = new Plane(planeTexture, 5f);
            circleHitbox = new Rectangle(plane.Location.ToPoint(), (new Vector2(Convert.ToSingle(planeTexture.Width * 0.1), Convert.ToSingle(planeTexture.Width * 0.1)).ToPoint()));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            planeTexture = Content.Load<Texture2D>("airplane");
            circkleTexture = Content.Load<Texture2D>("circle");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            circleHitbox.Location = plane.Update(_graphics).ToPoint();
            circleHitbox.Offset(-circleHitbox.Width/2, -circleHitbox.Height/2);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(circkleTexture, circleHitbox, Color.White);
            plane.Draw(_spriteBatch);
            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}